namespace OwlShed.Hushit.Data.AudioFiles;

internal sealed partial class AudioFileRepository : JsonDataRepositoryBase<IAudioFileInfo, MutableAudioFile, AudioFileUpdate, AudioFileInfo, AudioFileRepository.JsonModel>, IAudioFileRepository
{
	#region Nested types
	[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
	[JsonSerializable(typeof(JsonModel))]
	[JsonSerializable(typeof(string[]))]
	private sealed partial class JsonContext : JsonSerializerContext { }
	internal sealed class JsonModel
	{
		#region Properties
		[JsonPropertyName("path")]
		public required string Path { get; init; }

		[JsonPropertyName("hash")]
		public required string Hash { get; init; }

		[JsonPropertyName("track_name")]
		public required string TrackName { get; init; }

		[JsonPropertyName("track_date")]
		public required string? TrackDate { get; init; }

		[JsonPropertyName("track_id")]
		public required string? TrackId { get; init; }

		[JsonPropertyName("album_name")]
		public required string? AlbumName { get; init; }

		[JsonPropertyName("album_date")]
		public required string? AlbumDate { get; init; }

		[JsonPropertyName("track_artists")]
		public required string[] TrackArtists { get; init; }

		[JsonPropertyName("album_artists")]
		public required string[] AlbumArtists { get; init; }

		[JsonPropertyName("track_genres")]
		public required string[] TrackGenres { get; init; }

		[JsonPropertyName("album_genres")]
		public required string[] AlbumGenres { get; init; }

		[JsonPropertyName("duration")]
		public required double? Duration { get; init; }

		[JsonPropertyName("track_number")]
		public required int? TrackNumber { get; init; }

		[JsonPropertyName("total_tracks")]
		public required int? TotalTracks { get; init; }

		[JsonPropertyName("container_format")]
		public required string? ContainerFormat { get; init; }

		[JsonPropertyName("audio_format")]
		public required string? AudioFormat { get; init; }

		[JsonPropertyName("bit_rate")]
		public required int? BitRate { get; init; }

		[JsonPropertyName("sample_rate")]
		public required int? SampleRate { get; init; }

		[JsonPropertyName("channels")]
		public required int? Channels { get; init; }
		#endregion
	}
	#endregion

	#region Constants
	private const string PreferredHash = "sha256";
	#endregion

	#region Properties
	protected override JsonTypeInfo<JsonModel> TypeInfo => JsonContext.Default.JsonModel;
	#endregion

	#region Constructors
	public AudioFileRepository(IHushitData data, string baseDirectory) : base(data, baseDirectory)
	{
	}
	#endregion

	#region Persist methods
	public async ValueTask<IAudioFileInfo> CreateAsync(string path, CancellationToken cancellation = default)
	{
		return await CreateAsync(async (mutable, cancellation) =>
		{
			HashInfo hash = await GetHashAsync(path, PreferredHash, cancellation).ConfigureAwait(false);
			string trackName = Path.GetFileNameWithoutExtension(path);

			mutable
				.WithPath(path)
				.WithHash(hash)
				.WithTrackName(trackName);

			await AudioMetadataExtractor.ExtractAsync(path, mutable, cancellation).ConfigureAwait(false);

		}, cancellation).ConfigureAwait(false);
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
			await AudioMetadataExtractor.ExtractAsync(file.Path, mutable, cancellation).ConfigureAwait(false);
		});
	}
	#endregion

	#region Json methods
	protected override JsonModel ToJson(MutableAudioFile mutable)
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
		};

	}
	protected override MutableAudioFile FromJson(JsonModel json)
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
		};
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

