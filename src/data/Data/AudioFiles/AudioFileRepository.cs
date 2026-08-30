namespace OwlShed.Hushit.Data.AudioFiles;

internal sealed partial class AudioFileRepository : JsonDataRepositoryBase<IAudioFileInfo, MutableAudioFile, AudioFileUpdate, AudioFileInfo, AudioFileRepository.AudioFileJson>, IAudioFileRepository
{
	#region Nested types
	[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
	[JsonSerializable(typeof(AudioFileJson))]
	[JsonSerializable(typeof(string[]))]
	private sealed partial class AudioFileJsonContext : JsonSerializerContext { }
	internal sealed class AudioFileJson
	{
		#region Properties
		[JsonPropertyName("path")]
		public required string Path { get; init; }

		[JsonPropertyName("hash")]
		public required string Hash { get; init; }

		[JsonPropertyName("track_name")]
		public required string TrackName { get; init; }

		[JsonPropertyName("track_date")]
		public string? TrackDate { get; init; }

		[JsonPropertyName("track_id")]
		public string? TrackId { get; init; }

		[JsonPropertyName("album_name")]
		public string? AlbumName { get; init; }

		[JsonPropertyName("album_date")]
		public string? AlbumDate { get; init; }

		[JsonPropertyName("track_artists")]
		public string[] TrackArtists { get => field ??= []; init; }

		[JsonPropertyName("album_artists")]
		public string[] AlbumArtists { get => field ??= []; init; }

		[JsonPropertyName("track_genres")]
		public string[] TrackGenres { get => field ??= []; init; }

		[JsonPropertyName("album_genres")]
		public string[] AlbumGenres { get => field ??= []; init; }

		[JsonPropertyName("duration")]
		public double? Duration { get; init; }

		[JsonPropertyName("track_number")]
		public int? TrackNumber { get; init; }

		[JsonPropertyName("total_tracks")]
		public int? TotalTracks { get; init; }

		[JsonPropertyName("container_format")]
		public string? ContainerFormat { get; init; }

		[JsonPropertyName("audio_format")]
		public string? AudioFormat { get; init; }

		[JsonPropertyName("bit_rate")]
		public int? BitRate { get; init; }

		[JsonPropertyName("sample_rate")]
		public int? SampleRate { get; init; }

		[JsonPropertyName("channels")]
		public int? Channels { get; init; }

		[JsonPropertyName("fingerprint")]
		public string? Fingerprint { get; init; }

		[JsonPropertyName("cover_image_id")]
		public string? CoverImageId { get; init; }
		#endregion
	}
	#endregion

	#region Constants
	private const string PreferredHash = "sha256";
	#endregion

	#region Properties
	protected override JsonTypeInfo<AudioFileJson> TypeInfo => AudioFileJsonContext.Default.AudioFileJson;
	private JsonDataIndex<string> PathIndex { get; }
	#endregion

	#region Constructors
	public AudioFileRepository(IHushitData data, string baseDirectory) : base(data, baseDirectory)
	{
		PathIndex = new(IndexDirectory, "by_path");
	}
	#endregion

	#region Persist methods
	public async ValueTask<IAudioFileInfo> CreateAsync(string path, CancellationToken cancellation = default)
	{
		path = Path.GetNormalised(path);

		string id = CreateNewId();
		string pathId = await PathIndex.GetOrAddAsync(path, id, cancellation).ConfigureAwait(false);

		IAudioFileInfo? file = null;
		if (pathId != id)
		{
			file = await TryGetAsync(pathId, cancellation).ConfigureAwait(false);
			if (file is not null)
			{
				await TryReloadAsync(file, cancellation).ConfigureAwait(false);
				return file;
			}
			await PathIndex.RemoveAsync(pathId, cancellation).ConfigureAwait(false);
		}

		file = await CreateAsync(id, async (mutable, cancellation) =>
		{
			HashInfo hash = await GetHashAsync(path, PreferredHash, cancellation).ConfigureAwait(false);
			string trackName = Path.GetFileNameWithoutExtension(path);

			mutable.Path = path;
			mutable.Hash = hash;
			mutable.TrackName = trackName;

			Task extractTask = AudioMetadataExtractor.ExtractAsync(path, mutable, cancellation).AsTask();
			Task<IImageInfo?> coverTask = ExtractCoverAsync(path, cancellation).AsTask();

			await Task.WhenAll(extractTask, coverTask).ConfigureAwait(false);

			IImageInfo? cover = await coverTask;
			mutable.CoverImageId = cover?.Id;

		}, cancellation).ConfigureAwait(false);

		return file;
	}
	public async ValueTask<bool> TryReloadAsync(IAudioFileInfo file, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		return await ReloadAsync(file, force: false, cancellation).ConfigureAwait(false);
	}
	public async ValueTask<bool> ReloadAsync(IAudioFileInfo file, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		return await ReloadAsync(file, force: true, cancellation).ConfigureAwait(false);
	}
	private async ValueTask<bool> ReloadAsync(IAudioFileInfo file, bool force, CancellationToken cancellation = default)
	{
		if (File.Exists(file.Path) is false)
			return false;

		HashInfo newHash = await GetHashAsync(file.Path, file.Hash.Kind, cancellation).ConfigureAwait(false);

		if (force is false)
		{
			Debug.Assert(newHash.Kind == file.Hash.Kind);
			if (newHash.Value == file.Hash.Value)
				return false;
		}

		if (newHash.Kind != PreferredHash)
			newHash = await GetHashAsync(file.Path, PreferredHash, cancellation).ConfigureAwait(false);

		return await UpdateAsync(file.Id, async (mutable, cancellation) =>
		{
			mutable.Hash = newHash;
			Task extractTask = AudioMetadataExtractor.ExtractAsync(file.Path, mutable, cancellation).AsTask();
			Task<IImageInfo?> coverTask = ExtractCoverAsync(file.Path, cancellation).AsTask();

			await Task.WhenAll(extractTask, coverTask).ConfigureAwait(false);

			IImageInfo? cover = await coverTask;
			mutable.CoverImageId = cover?.Id;
		});
	}
	protected override async ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default)
	{
		await base.StopPersistingAsync(id, directory, cancellation).ConfigureAwait(false);
		await PathIndex.RemoveAsync(id, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Json methods
	protected override AudioFileJson ToJson(MutableAudioFile mutable)
	{
		Debug.Assert(mutable.Path is not null);
		Debug.Assert(mutable.Hash is not null);
		Debug.Assert(mutable.TrackName is not null);

		return new()
		{
			Path = mutable.Path,
			Hash = mutable.Hash.Value.ToString(),
			TrackName = mutable.TrackName,
			TrackDate = mutable.TrackDate?.ToString(),
			TrackId = mutable.TrackId,
			AlbumName = mutable.AlbumName,
			AlbumDate = mutable.AlbumDate?.ToString(),
			TrackArtists = mutable.TrackArtists.ToArray(),
			AlbumArtists = mutable.AlbumArtists.ToArray(),
			TrackGenres = mutable.TrackGenres.ToArray(),
			AlbumGenres = mutable.AlbumGenres.ToArray(),
			Duration = mutable.Duration?.TotalSeconds,
			TrackNumber = mutable.TrackNumber,
			TotalTracks = mutable.TotalTracks,
			ContainerFormat = mutable.ContainerFormat,
			AudioFormat = mutable.AudioFormat,
			BitRate = mutable.BitRate,
			SampleRate = mutable.SampleRate,
			Channels = mutable.Channels,
			Fingerprint = mutable.Fingerprint?.ToString(),
			CoverImageId = mutable.CoverImageId,
		};

	}
	protected override MutableAudioFile FromJson(AudioFileJson json)
	{
		return new()
		{
			Path = json.Path,
			Hash = HashInfo.TryParse(json.Hash),
			TrackName = json.TrackName,
			TrackId = json.TrackId,
			TrackDate = DateInfo.TryParse(json.TrackDate),
			AlbumName = json.AlbumName,
			AlbumDate = DateInfo.TryParse(json.AlbumDate),
			TrackArtists = json.TrackArtists,
			AlbumArtists = json.AlbumArtists,
			TrackGenres = json.TrackGenres,
			AlbumGenres = json.AlbumGenres,
			Duration = json.Duration is null ? null : TimeSpan.FromSeconds(json.Duration.Value),
			TrackNumber = json.TrackNumber,
			TotalTracks = json.TotalTracks,
			ContainerFormat = json.ContainerFormat,
			AudioFormat = json.AudioFormat,
			BitRate = json.BitRate,
			SampleRate = json.SampleRate,
			Channels = json.Channels,
			Fingerprint = FingerprintInfo.TryParse(json.Fingerprint),
			CoverImageId = json.CoverImageId,
		};
	}
	#endregion

	#region Helpers
	private async ValueTask<IImageInfo?> ExtractCoverAsync(string audioPath, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string? imagePath = await AudioMetadataExtractor.ExtractCoverAsync(audioPath, cancellation).ConfigureAwait(false);
		if (imagePath is null)
			return null;

		IImageInfo? image = await Data.Images.CreateAsync(imagePath, cancellation).ConfigureAwait(false);
		File.Delete(imagePath);

		string? directory = Path.GetDirectoryName(imagePath);
		Directory.DeleteIfEmpty(directory);

		return image;
	}
	#endregion

	#region Hash methods
	private static async ValueTask<HashInfo> GetHashAsync(string path, string kind, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string hash = kind switch
		{
			"sha256" => await Hash.Sha256FromFileAsync(path, cancellation).ConfigureAwait(false),

			_ => ThrowHelper.ThrowArgumentException<string>(nameof(kind), $"Unknown hash kind ({kind})."),
		};

		return new(kind, hash);
	}
	#endregion

	#region Helpers
	protected override AudioFileInfo Create(string id, MutableAudioFile initialState) => new(Data, id, initialState);
	protected override void CopyState(AudioFileInfo model, MutableAudioFile oldState, MutableAudioFile newState, AudioFileUpdate update)
	{
		model.CopyState(newState);
	}
	protected override void MarkAsDeleted(AudioFileInfo model) => model.Exists = false;
	#endregion
}

