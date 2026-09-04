namespace OwlShed.Hushit.Data.Genres;

internal sealed partial class GenreRepository : JsonDataRepositoryBase<IGenreInfo, MutableGenre, GenreUpdate, GenreInfo, GenreRepository.GenreJson>, IGenreRepository
{
	#region Nested types
	[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
	[JsonSerializable(typeof(GenreJson))]
	private sealed partial class GenreJsonContext : JsonSerializerContext { }
	internal sealed class GenreJson
	{
		#region Properties
		[JsonPropertyName("name")]
		public required string Name { get; init; }
		#endregion
	}
	#endregion

	#region Constants
	private const string TrackBackRefName = "tracks";
	private const string AlbumBackRefName = "albums";
	private const string ArtistBackRefName = "artists";
	#endregion

	#region Properties
	protected override JsonTypeInfo<GenreJson> TypeInfo => GenreJsonContext.Default.GenreJson;
	private JsonDataIndex<string> NameIndex { get; }
	private ValueLock<string> NameLock { get; } = new();
	private ValueLock<GenreInfo> GenreLock { get; } = new();
	#endregion

	#region Constructors
	public GenreRepository(IHushitData data, string baseDirectory) : base(data, baseDirectory)
	{
		NameIndex = new(IndexDirectory, "by_name");
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
		HashSet<string> removedGenres = update.Old.GenreIds.Except(update.New.GenreIds).ToHashSet();
		HashSet<string> addedGenres = update.New.GenreIds.Except(update.Old.GenreIds).ToHashSet();

		foreach (string id in removedGenres)
		{
			GenreInfo? genre = await TryGetCoreAsync(id, cancellation).ConfigureAwait(false);
			if (genre is not null)
			{
				await using (await GenreLock.LockAsync(genre, cancellation).ConfigureAwait(false))
				{
					genre.TrackIds.Remove(update.Model.Id);
					await SaveTrackBackReferencesAsync(genre, cancellation).ConfigureAwait(false);
				}
			}
		}

		foreach (string id in addedGenres)
		{
			GenreInfo? genre = await TryGetCoreAsync(id, cancellation).ConfigureAwait(false);
			if (genre is not null)
			{
				await using (await GenreLock.LockAsync(genre, cancellation).ConfigureAwait(false))
				{
					genre.TrackIds.Add(update.Model.Id);
					await SaveTrackBackReferencesAsync(genre, cancellation).ConfigureAwait(false);
				}
			}
		}
	}
	#endregion

	#region Backreference methods
	private async ValueTask SaveTrackBackReferencesAsync(IGenreInfo genre, CancellationToken cancellation = default)
	{
		await SaveBackreferencesAsync(genre.Id, TrackBackRefName, genre.TrackIds, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Persist methods
	public async ValueTask<IGenreInfo> GetOrCreateAsync(string name, CancellationToken cancellation = default)
	{
		await using IAsyncDisposable _ = await NameLock.LockAsync(name, cancellation).ConfigureAwait(false);

		string id = CreateNewId();
		string nameId = await NameIndex.GetOrAddAsync(name, id, cancellation).ConfigureAwait(false);

		IGenreInfo? genre;
		if (nameId != id)
		{
			genre = await TryGetAsync(nameId, cancellation).ConfigureAwait(false);
			if (genre is not null)
				return genre;

			await NameIndex.RemoveAsync(nameId, cancellation).ConfigureAwait(false);
		}

		genre = await CreateAsync(id, async (mutable, cancellation) =>
		{
			mutable.Name = name;
		}).ConfigureAwait(false);

		return genre;
	}
	protected override async ValueTask OnCreatedAsync(GenreInfo model, CancellationToken cancellation = default)
	{
		await base.OnCreatedAsync(model, cancellation).ConfigureAwait(false);
		await NameIndex.SetAsync(model.Id, model.Name, cancellation).ConfigureAwait(false);
	}
	protected override async ValueTask OnUpdateAsync(GenreInfo model, MutableGenre oldState, MutableGenre newState, GenreUpdate update, CancellationToken cancellation = default)
	{
		await base.OnUpdateAsync(model, oldState, newState, update, cancellation).ConfigureAwait(false);
		await NameIndex.SetAsync(model.Id, model.Name, cancellation).ConfigureAwait(false);
	}
	protected override async ValueTask<GenreInfo?> TryLoadPersistedAsync(string id, string directory, CancellationToken cancellation = default)
	{
		GenreInfo? genre = await base.TryLoadPersistedAsync(id, directory, cancellation).ConfigureAwait(false);
		if (genre is null)
			return null;

		IReadOnlyList<string> tracks = await LoadBackreferencesAsync(id, TrackBackRefName, cancellation).ConfigureAwait(false);
		IReadOnlyList<string> albums = await LoadBackreferencesAsync(id, AlbumBackRefName, cancellation).ConfigureAwait(false);
		IReadOnlyList<string> artists = await LoadBackreferencesAsync(id, ArtistBackRefName, cancellation).ConfigureAwait(false);

		genre.TrackIds.Replace(tracks);
		genre.AlbumIds.Replace(albums);
		genre.ArtistIds.Replace(artists);

		return genre;
	}
	protected override async ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default)
	{
		await base.StopPersistingAsync(id, directory, cancellation).ConfigureAwait(false);

		await DeleteBackreferencesAsync(id, TrackBackRefName, cancellation).ConfigureAwait(false);
		await DeleteBackreferencesAsync(id, AlbumBackRefName, cancellation).ConfigureAwait(false);
		await DeleteBackreferencesAsync(id, ArtistBackRefName, cancellation).ConfigureAwait(false);

		await NameIndex.RemoveAsync(id, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Json methods
	protected override GenreJson ToJson(MutableGenre mutable)
	{
		Debug.Assert(mutable.Name is not null);

		return new()
		{
			Name = mutable.Name,
		};
	}
	protected override MutableGenre FromJson(GenreJson json)
	{
		return new()
		{
			Name = json.Name
		};
	}
	#endregion

	#region Helpers
	protected override GenreInfo Create(string id, MutableGenre initialState) => new(Data, id, initialState);
	protected override void CopyState(GenreInfo model, MutableGenre oldState, MutableGenre newState, GenreUpdate update) => model.CopyState(newState);
	protected override void MarkAsDeleted(GenreInfo model) => model.Exists = false;
	#endregion
}
