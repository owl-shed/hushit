namespace OwlShed.Hushit.Data.AudioFiles;

/// <summary>
/// 	Represents a mutable version of the <see cref="IAudioFileInfo"/>.
/// </summary>
public sealed class MutableAudioFile : MutableDataModelBase<MutableAudioFile, AudioFileUpdate>
{
	#region Properties
	/// <inheritdoc cref="IAudioFileInfo.Path"/>
	public string? Path { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.Hash"/>
	public HashInfo? Hash { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackName"/>
	public string? TrackName { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackId"/>
	public string? TrackId { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackDate"/>
	public DateInfo? TrackDate { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.AlbumName"/>
	public string? AlbumName { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.AlbumDate"/>
	public DateInfo? AlbumDate { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackArtists"/>
	public IList<string> TrackArtists { get; set; } = [];

	/// <inheritdoc cref="IAudioFileInfo.AlbumArtists"/>
	public IList<string> AlbumArtists { get; set; } = [];

	/// <inheritdoc cref="IAudioFileInfo.TrackGenres"/>
	public IList<string> TrackGenres { get; set; } = [];

	/// <inheritdoc cref="IAudioFileInfo.AlbumGenres"/>
	public IList<string> AlbumGenres { get; set; } = [];

	/// <inheritdoc cref="IAudioFileInfo.Duration"/>
	public TimeSpan? Duration { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackNumber"/>
	public int? TrackNumber { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TotalTracks"/>
	public int? TotalTracks { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.ContainerFormat"/>
	public string? ContainerFormat { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.AudioFormat"/>
	public string? AudioFormat { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.BitRate"/>
	public int? BitRate { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.SampleRate"/>
	public int? SampleRate { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.Channels"/>
	public int? Channels { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.Fingerprint"/>
	public FingerprintInfo? Fingerprint { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.CoverImageId"/>
	public string? CoverImageId { get; set; }
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override AudioFileUpdate GetUpdateFrom(MutableAudioFile oldState)
	{
		if (Path is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Path)}' to have a value.");

		if (Hash is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Hash)}' to have a value.");

		if (TrackName is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(TrackName)}' to have a value.");

		return new()
		{
			Path = Update.Value(oldState.Path, Path),
			Hash = oldState.Hash is null ? Hash.Value : Update.Value(oldState.Hash, Hash),
			TrackName = Update.Value(oldState.TrackName, TrackName),
			TrackId = Update.Nullable(oldState.TrackId, TrackId),
			TrackDate = Update.Nullable(oldState.TrackDate, TrackDate),
			AlbumName = Update.Nullable(oldState.AlbumName, AlbumName),
			AlbumDate = Update.Nullable(oldState.AlbumDate, AlbumDate),
			TrackArtists = Update.List(oldState.TrackArtists, TrackArtists),
			AlbumArtists = Update.List(oldState.AlbumArtists, AlbumArtists),
			TrackGenres = Update.List(oldState.TrackGenres, TrackGenres),
			AlbumGenres = Update.List(oldState.AlbumGenres, AlbumGenres),
			Duration = Update.Nullable(oldState.Duration, Duration),
			TrackNumber = Update.Nullable(oldState.TrackNumber, TrackNumber),
			TotalTracks = Update.Nullable(oldState.TotalTracks, TotalTracks),
			ContainerFormat = Update.Nullable(oldState.ContainerFormat, ContainerFormat),
			AudioFormat = Update.Nullable(oldState.AudioFormat, AudioFormat),
			BitRate = Update.Nullable(oldState.BitRate, BitRate),
			SampleRate = Update.Nullable(oldState.SampleRate, SampleRate),
			Channels = Update.Nullable(oldState.Channels, Channels),
			Fingerprint = Update.Nullable(oldState.Fingerprint, Fingerprint),
			CoverImageId = Update.Nullable(oldState.CoverImageId, CoverImageId)
		};
	}
	#endregion
}
