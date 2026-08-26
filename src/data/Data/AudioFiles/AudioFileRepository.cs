namespace OwlShed.Hushit.Data.AudioFiles;

internal sealed partial class AudioFileRepository : LocalDataRepositoryBase<IAudioFileInfo, MutableAudioFile, AudioFileUpdate, AudioFileInfo>, IAudioFileRepository
{
	#region Nested types
	[JsonSourceGenerationOptions(WriteIndented = true)]
	[JsonSerializable(typeof(JsonModel))]
	[JsonSerializable(typeof(HashInfo))]
	[JsonSerializable(typeof(DateInfo))]
	[JsonSerializable(typeof(string[]))]
	private sealed partial class JsonContext : JsonSerializerContext { }
	private sealed class JsonModel
	{
		#region Properties
		[JsonPropertyName("path")]
		public required string Path { get; init; }

		[JsonPropertyName("hash")]
		public required HashInfo Hash { get; init; }

		[JsonPropertyName("track_name")]
		public required string TrackName { get; init; }

		[JsonPropertyName("track_date")]
		public required DateInfo? TrackDate { get; init; }

		[JsonPropertyName("track_id")]
		public required string? TrackId { get; init; }

		[JsonPropertyName("album_name")]
		public required string? AlbumName { get; init; }

		[JsonPropertyName("album_date")]
		public required DateInfo? AlbumDate { get; init; }

		[JsonPropertyName("track_artists")]
		public required string[] TrackArtists { get; init; }

		[JsonPropertyName("album_artists")]
		public required string[] AlbumArtists { get; init; }

		[JsonPropertyName("track_genres")]
		public required string[] TrackGenres { get; init; }

		[JsonPropertyName("album_genres")]
		public required string[] AlbumGenres { get; init; }
		#endregion
	}
	#endregion

	#region Constants
	private const string FileName = "data.json";
	#endregion

	#region Constructors
	public AudioFileRepository(IHushitData data, string baseDirectory) : base(data, baseDirectory)
	{
	}
	#endregion

	#region Persist methods
	public async ValueTask<IAudioFileInfo> CreateAsync(string path, CancellationToken cancellation = default)
	{
		return await CreateAsync(mutable =>
		{
			HashInfo hash = new("fake", "fake");
			string trackName = Path.GetFileNameWithoutExtension(path);

			mutable
				.WithPath(path)
				.WithHash(hash)
				.WithTrackName(trackName);

		}, cancellation).ConfigureAwait(false);
	}
	public ValueTask<bool> TryReloadAsync(IAudioFileInfo file, CancellationToken cancellation = default) => throw new NotImplementedException();
	public ValueTask<bool> ReloadAsync(IAudioFileInfo file, CancellationToken cancellation = default) => throw new NotImplementedException();
	protected override async ValueTask PersistAsync(AudioFileInfo model, string directory, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		JsonModel json = new()
		{
			Path = model.Path,
			Hash = model.Hash,
			TrackName = model.TrackName,
			TrackDate = model.TrackDate,
			TrackId = model.TrackId,
			AlbumName = model.AlbumName,
			AlbumDate = model.AlbumDate,
			TrackArtists = model.TrackArtists.ToArray(),
			AlbumArtists = model.AlbumArtists.ToArray(),
			TrackGenres = model.TrackGenres.ToArray(),
			AlbumGenres = model.AlbumGenres.ToArray(),
		};

		string path = GetJsonPath(directory);
		using (FileStream file = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
			await JsonSerializer.SerializeAsync(file, json, JsonContext.Default.JsonModel, cancellation).ConfigureAwait(false);
	}
	protected override async ValueTask<AudioFileInfo?> TryLoadPersistedAsync(string id, string directory, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string path = GetJsonPath(directory);
		JsonModel? json;
		using (FileStream file = File.OpenRead(path))
			json = await JsonSerializer.DeserializeAsync(file, JsonContext.Default.JsonModel, cancellation).ConfigureAwait(false);

		// Todo(Nightowl): Log corrupted file;
		if (json is null)
			return null;

		MutableAudioFile mutable = new()
		{
			Path = json.Path,
			Hash = json.Hash,
			TrackName = json.TrackName,
			TrackId = json.TrackId,
			TrackDate = json.TrackDate,
			AlbumName = json.AlbumName,
			AlbumDate = json.AlbumDate,
			TrackArtists = json.TrackArtists,
			AlbumArtists = json.AlbumArtists,
			TrackGenres = json.TrackGenres,
			AlbumGenres = json.AlbumGenres
		};

		AudioFileInfo model = Create(id, mutable);
		return model;
	}
	protected override ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string path = GetJsonPath(directory);
		File.TryDelete(path);

		return default;
	}
	#endregion

	#region Helpers
	private static string GetJsonPath(string directory) => Path.Combine(directory, FileName);
	protected override AudioFileInfo Create(string id, MutableAudioFile initialState) => new(Data, id, initialState);
	protected override void CopyState(AudioFileInfo model, MutableAudioFile oldState, MutableAudioFile newState, AudioFileUpdate update)
	{
		model.CopyState(newState);
	}
	protected override void MarkAsDeleted(AudioFileInfo model) => model.Exists = false;
	#endregion
}

