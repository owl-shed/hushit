namespace OwlShed.Hushit.Data.Albums;

internal sealed partial class AlbumRepository : JsonDataRepositoryBase<IAlbumInfo, MutableAlbum, AlbumUpdate, AlbumInfo, AlbumRepository.AlbumJson>, IAlbumRepository
{
	#region Nested types
	[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
	[JsonSerializable(typeof(AlbumJson))]
	[JsonSerializable(typeof(AlbumKey))]
	[JsonSerializable(typeof(Dictionary<string, AlbumKey>))]
	private sealed partial class AlbumJsonContext : JsonSerializerContext { }
	internal sealed class AlbumJson
	{
		#region Properties
		[JsonPropertyName("name")]
		public required string Name { get; init; }

		[JsonPropertyName("artist_ids")]
		public string[] ArtistIds { get => field ??= []; init; }
		#endregion
	}
	private sealed class AlbumKey : IEquatable<AlbumKey>
	{
		#region Properties
		[JsonPropertyName("name")]
		public string Name { get; init; }

		[JsonPropertyName("artist_ids")]
		public string[] ArtistIds { get; }
		#endregion

		#region Constructors
		[JsonConstructor]
		public AlbumKey(string name, string[] artistIds) : this(name, (IReadOnlyCollection<string>)artistIds) { }
		public AlbumKey(string name, IReadOnlyCollection<string> artistIds)
		{
			Name = name;
			ArtistIds = artistIds.Order(StringComparer.Ordinal).ToArray();
		}
		#endregion

		#region Methods
		public bool Equals([NotNullWhen(true)] AlbumKey? other)
		{
			if (other is null)
				return false;

			if (Name != other.Name)
				return false;

			if (ArtistIds.Length != other.ArtistIds.Length)
				return false;

			for (int i = 0; i < ArtistIds.Length; i++)
			{
				if (ArtistIds[i] != other.ArtistIds[i])
					return false;
			}

			return true;
		}
		public override bool Equals(object? obj)
		{
			if (obj is AlbumKey other)
				return Equals(other);

			return false;
		}
		public override int GetHashCode()
		{
			HashCode code = new();
			code.Add(Name);
			code.Add(ArtistIds.Length);

			foreach (string artist in ArtistIds)
				code.Add(artist);

			return code.ToHashCode();
		}
		#endregion
	}
	#endregion

	#region Constants
	private const string TrackBackRefName = "tracks";
	private const string GenreBackRefName = "genres";
	#endregion

	#region Properties
	protected override JsonTypeInfo<AlbumJson> TypeInfo => AlbumJsonContext.Default.AlbumJson;
	private JsonDataIndex<AlbumKey> AlbumKeyIndex { get; }
	private ValueLock<AlbumKey> AlbumKeyLock { get; } = new();
	private ValueLock<AlbumInfo> AlbumLock { get; } = new();
	#endregion

	#region Constructors
	public AlbumRepository(IHushitData data, string baseDirectory) : base(data, baseDirectory)
	{
		AlbumKeyIndex = new(IndexDirectory, "by_name_and_artists", AlbumJsonContext.Default.DictionaryStringAlbumKey);
	}
	#endregion

	#region Methods
	public override void Initialise()
	{
		base.Initialise();
		Data.Tracks.ModelUpdated.Subscribe(TrackUpdatedAsync);
	}
	#endregion

	#region React methods
	private async ValueTask TrackUpdatedAsync(ModelUpdateInfo<ITrackInfo, MutableTrack, TrackUpdate> update, CancellationToken cancellation)
	{
		if (update.Update.AlbumId.HasChanged)
		{
			AlbumInfo? oldAlbum = update.Old.AlbumId is null ? null : await TryGetCoreAsync(update.Old.AlbumId, cancellation).ConfigureAwait(false);
			if (oldAlbum is not null)
			{
				await using (await AlbumLock.LockAsync(oldAlbum, cancellation).ConfigureAwait(false))
				{
					oldAlbum.TrackIds.Remove(update.Model.Id);
					await SaveTrackBackReferencesAsync(oldAlbum, cancellation).ConfigureAwait(false);
				}
			}

			AlbumInfo? newAlbum = update.New.AlbumId is null ? null : await TryGetCoreAsync(update.New.AlbumId, cancellation).ConfigureAwait(false);
			if (newAlbum is not null)
			{
				await using (await AlbumLock.LockAsync(newAlbum, cancellation).ConfigureAwait(false))
				{
					newAlbum.TrackIds.Add(update.Model.Id);
					await SaveTrackBackReferencesAsync(newAlbum, cancellation).ConfigureAwait(false);
				}
			}
		}
	}
	#endregion

	#region Backreference methods
	private async ValueTask SaveTrackBackReferencesAsync(IAlbumInfo album, CancellationToken cancellation = default)
	{
		await SaveBackreferencesAsync(album.Id, TrackBackRefName, album.TrackIds, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Persist methods
	/// <inheritdoc/>
	public async ValueTask<IAlbumInfo> GetOrCreateAsync(string name, IReadOnlyCollection<string> artistIds, CancellationToken cancellation = default)
	{
		AlbumKey key = new(name, artistIds);
		await using IAsyncDisposable _ = await AlbumKeyLock.LockAsync(key, cancellation).ConfigureAwait(false);

		string id = CreateNewId();
		string albumKeyId = await AlbumKeyIndex.GetOrAddAsync(key, id, cancellation).ConfigureAwait(false);

		IAlbumInfo? album;
		if (albumKeyId != id)
		{
			album = await TryGetAsync(albumKeyId, cancellation).ConfigureAwait(false);
			if (album is not null)
				return album;
		}

		album = await CreateAsync(id, async (mutable, cancellation) =>
		{
			mutable.Name = name;
			mutable.ArtistIds = [.. artistIds];
		}).ConfigureAwait(false);

		return album;
	}
	protected override async ValueTask OnCreatedAsync(AlbumInfo model, CancellationToken cancellation = default)
	{
		await base.OnCreatedAsync(model, cancellation).ConfigureAwait(false);

		AlbumKey key = new(model.Name, model.ArtistIds);
		await AlbumKeyIndex.SetAsync(model.Id, key, cancellation).ConfigureAwait(false);
	}
	protected override async ValueTask<AlbumInfo?> TryLoadPersistedAsync(string id, string directory, CancellationToken cancellation = default)
	{
		AlbumInfo? album = await base.TryLoadPersistedAsync(id, directory, cancellation).ConfigureAwait(false);
		if (album is null)
			return null;

		IReadOnlyList<string> tracks = await LoadBackreferencesAsync(id, TrackBackRefName, cancellation).ConfigureAwait(false);
		IReadOnlyList<string> genres = await LoadBackreferencesAsync(id, GenreBackRefName, cancellation).ConfigureAwait(false);

		album.TrackIds.Replace(tracks);
		album.GenreIds.Replace(genres);

		return album;
	}
	protected override async ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default)
	{
		await base.StopPersistingAsync(id, directory, cancellation).ConfigureAwait(false);

		await DeleteBackreferencesAsync(id, TrackBackRefName, cancellation).ConfigureAwait(false);
		await DeleteBackreferencesAsync(id, GenreBackRefName, cancellation).ConfigureAwait(false);

		await AlbumKeyIndex.RemoveAsync(id, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Json methods
	protected override AlbumJson ToJson(MutableAlbum mutable)
	{
		Debug.Assert(mutable.Name is not null);

		return new()
		{
			Name = mutable.Name,
			ArtistIds = mutable.ArtistIds.ToArray()
		};
	}
	protected override MutableAlbum FromJson(AlbumJson json)
	{
		return new()
		{
			Name = json.Name,
			ArtistIds = json.ArtistIds,
		};
	}
	#endregion

	#region Helpers
	protected override AlbumInfo Create(string id, MutableAlbum initialState) => new(Data, id, initialState);
	protected override void CopyState(AlbumInfo model, MutableAlbum oldState, MutableAlbum newState, AlbumUpdate update) => model.CopyState(newState);
	protected override void MarkAsDeleted(AlbumInfo model) => model.Exists = false;
	#endregion
}
