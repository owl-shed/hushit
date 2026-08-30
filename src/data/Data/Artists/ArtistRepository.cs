namespace OwlShed.Hushit.Data.Artists;

internal sealed partial class ArtistRepository : JsonDataRepositoryBase<IArtistInfo, MutableArtist, ArtistUpdate, ArtistInfo, ArtistRepository.ArtistJson>, IArtistRepository
{
	#region Nested types
	[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
	[JsonSerializable(typeof(ArtistJson))]
	private sealed partial class ArtistJsonContext : JsonSerializerContext { }
	internal sealed class ArtistJson
	{
		#region Properties
		[JsonPropertyName("name")]
		public required string Name { get; init; }

		[JsonPropertyName("aliases"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string[]? Aliases { get; init; }
		#endregion
	}
	#endregion

	#region Constants
	private const string AlbumBackRefName = "albums";
	private const string TrackBackRefName = "tracks";
	private const string GenreBackRefName = "genres";
	#endregion

	#region Properties
	protected override JsonTypeInfo<ArtistJson> TypeInfo => ArtistJsonContext.Default.ArtistJson;
	private JsonDataIndex<string> NameIndex { get; }
	#endregion

	#region Constructors
	public ArtistRepository(IHushitData data, string baseDirectory) : base(data, baseDirectory)
	{
		NameIndex = new(IndexDirectory, "by_name");
	}
	#endregion

	#region Persist methods
	public async ValueTask<IArtistInfo> GetOrCreateAsync(string name, CancellationToken cancellation = default)
	{
		string id = CreateNewId();
		string nameId = await NameIndex.GetOrAddAsync(name, id, cancellation).ConfigureAwait(false);

		IArtistInfo? artist;
		if (nameId != id)
		{
			artist = await TryGetAsync(nameId, cancellation).ConfigureAwait(false);
			if (artist is not null)
				return artist;

			await NameIndex.RemoveAsync(nameId, cancellation).ConfigureAwait(false);
		}

		artist = await CreateAsync(id, async (mutable, cancellation) =>
		{
			mutable.Name = name;

		}).ConfigureAwait(false);
		Debug.WriteLine($"Created new artist {artist.Id} - \"{artist.Name}\"");

		return artist;
	}
	protected override async ValueTask<ArtistInfo?> TryLoadPersistedAsync(string id, string directory, CancellationToken cancellation = default)
	{
		ArtistInfo? artist = await base.TryLoadPersistedAsync(id, directory, cancellation).ConfigureAwait(false);
		if (artist is null)
			return null;

		IReadOnlyList<string> albums = await LoadBackreferencesAsync(id, AlbumBackRefName, cancellation).ConfigureAwait(false);
		IReadOnlyList<string> tracks = await LoadBackreferencesAsync(id, TrackBackRefName, cancellation).ConfigureAwait(false);
		IReadOnlyList<string> genres = await LoadBackreferencesAsync(id, GenreBackRefName, cancellation).ConfigureAwait(false);

		artist.AlbumIds.Replace(albums);
		artist.TrackIds.Replace(tracks);
		artist.GenreIds.Replace(genres);

		return artist;
	}
	protected override async ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default)
	{
		await base.StopPersistingAsync(id, directory, cancellation).ConfigureAwait(false);

		await DeleteBackreferencesAsync(id, AlbumBackRefName, cancellation).ConfigureAwait(false);
		await DeleteBackreferencesAsync(id, TrackBackRefName, cancellation).ConfigureAwait(false);
		await DeleteBackreferencesAsync(id, GenreBackRefName, cancellation).ConfigureAwait(false);

		await NameIndex.RemoveAsync(id, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Json methods
	protected override ArtistJson ToJson(MutableArtist mutable)
	{
		Debug.Assert(mutable.Name is not null);

		return new()
		{
			Name = mutable.Name,
			Aliases = mutable.Aliases.Any() ? mutable.Aliases.ToArray() : null,
		};
	}
	protected override MutableArtist FromJson(ArtistJson json)
	{
		return new()
		{
			Name = json.Name,
			Aliases = json.Aliases ?? []
		};
	}
	#endregion

	#region Helpers
	protected override ArtistInfo Create(string id, MutableArtist initialState) => new(Data, id, initialState);
	protected override void CopyState(ArtistInfo model, MutableArtist oldState, MutableArtist newState, ArtistUpdate update) => model.CopyState(newState);
	protected override void MarkAsDeleted(ArtistInfo model) => model.Exists = false;
	#endregion
}
