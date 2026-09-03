namespace OwlShed.Hushit.Data.Tracks;

internal sealed partial class TrackRepository : JsonDataRepositoryBase<ITrackInfo, MutableTrack, TrackUpdate, TrackInfo, TrackRepository.TrackJson>, ITrackRepository
{
	#region Nested types
	[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
	[JsonSerializable(typeof(TrackJson))]
	private sealed partial class TrackJsonContext : JsonSerializerContext { }
	internal sealed class TrackJson
	{
		#region Properties
		[JsonPropertyName("name")]
		public required string Name { get; init; }

		[JsonPropertyName("duration")]
		public required double Duration { get; init; }

		[JsonPropertyName("album_id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? AlbumId { get; init; }

		[JsonPropertyName("genre_ids"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string[]? GenreIds { get; init; }

		[JsonPropertyName("artist_ids"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string[]? ArtistIds { get; init; }
		#endregion
	}
	#endregion

	#region Constants
	private const string AudioFileBackRefName = "audio_file";
	#endregion


	#region Properties
	protected override JsonTypeInfo<TrackJson> TypeInfo => TrackJsonContext.Default.TrackJson;
	private ValueLock<TrackInfo> TrackLock { get; } = new();
	#endregion

	#region Constructors
	public TrackRepository(IHushitData data, string baseDirectory) : base(data, baseDirectory)
	{
	}
	#endregion

	#region Methods
	public override void Initialise()
	{
		base.Initialise();
		Data.AudioFiles.ModelUpdated.Subscribe(AudioFileUpdatedAsync);
	}
	#endregion

	#region React methods
	private async ValueTask AudioFileUpdatedAsync(ModelUpdateInfo<IAudioFileInfo, MutableAudioFile, AudioFileUpdate> update, CancellationToken cancellation)
	{
		if (update.Update.TrackId.HasChanged)
		{
			TrackInfo? oldTrack = update.Old.TrackId is null ? null : await TryGetCoreAsync(update.Old.TrackId, cancellation).ConfigureAwait(false);
			if (oldTrack is not null)
			{
				await using (await TrackLock.LockAsync(oldTrack, cancellation).ConfigureAwait(false))
				{
					oldTrack.AudioFileId = update.Model.Id;
					await SaveAudioFileBackReferenceAsync(oldTrack, cancellation).ConfigureAwait(false);
				}
			}

			TrackInfo? newTrack = update.New.TrackId is null ? null : await TryGetCoreAsync(update.New.TrackId, cancellation).ConfigureAwait(false);
			if (newTrack is not null)
			{
				await using (await TrackLock.LockAsync(newTrack, cancellation).ConfigureAwait(false))
				{
					newTrack.AudioFileId = update.Model.Id;
					await SaveAudioFileBackReferenceAsync(newTrack, cancellation).ConfigureAwait(false);
				}
			}
		}
	}
	#endregion

	#region Backreference methods
	private async ValueTask SaveAudioFileBackReferenceAsync(ITrackInfo track, CancellationToken cancellation = default)
	{
		await SaveBackreferenceAsync(track.Id, AudioFileBackRefName, track.AudioFileId, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Persist methods
	protected override async ValueTask<TrackInfo?> TryLoadPersistedAsync(string id, string directory, CancellationToken cancellation = default)
	{
		TrackInfo? track = await base.TryLoadPersistedAsync(id, directory, cancellation).ConfigureAwait(false);
		if (track is null)
			return null;

		string? audioFileId = await LoadBackreferenceAsync(id, AudioFileBackRefName, cancellation).ConfigureAwait(false);
		track.AudioFileId = audioFileId;

		return track;
	}
	protected override async ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default)
	{
		await base.StopPersistingAsync(id, directory, cancellation).ConfigureAwait(false);
		await DeleteBackreferencesAsync(id, AudioFileBackRefName, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Json methods
	protected override TrackJson ToJson(MutableTrack mutable)
	{
		Debug.Assert(mutable.Name is not null);

		return new()
		{
			Name = mutable.Name,
			Duration = mutable.Duration.TotalSeconds,
			AlbumId = mutable.AlbumId,
			ArtistIds = mutable.ArtistIds.Count is 0 ? null : mutable.ArtistIds.ToArray(),
			GenreIds = mutable.GenreIds.Count is 0 ? null : mutable.GenreIds.ToArray(),
		};
	}
	protected override MutableTrack FromJson(TrackJson json)
	{
		return new()
		{
			Name = json.Name,
			Duration = TimeSpan.FromSeconds(json.Duration),
			AlbumId = json.AlbumId,
			ArtistIds = json.ArtistIds ?? [],
			GenreIds = json.GenreIds ?? [],
		};
	}
	#endregion

	#region Helpers
	protected override TrackInfo Create(string id, MutableTrack initialState) => new(Data, id, initialState);
	protected override void CopyState(TrackInfo model, MutableTrack oldState, MutableTrack newState, TrackUpdate update) => model.CopyState(newState);
	protected override void MarkAsDeleted(TrackInfo model) => model.Exists = false;
	#endregion
}
