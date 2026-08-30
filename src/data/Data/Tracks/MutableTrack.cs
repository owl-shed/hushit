namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents a mutable version of the <see cref="ITrackInfo"/>.
/// </summary>
public sealed class MutableTrack :
	MutableDataModelBase<MutableTrack, TrackUpdate>,
	IMutableArtistReferences,
	IMutableGenreReferences
{
	#region Properties
	/// <inheritdoc cref="ITrackInfo.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc cref="ITrackInfo.Duration"/>
	public TimeSpan? Duration { get; set; }

	/// <inheritdoc cref="ITrackInfo.AlbumId"/>
	public string? AlbumId { get; set; }

	/// <inheritdoc cref="ITrackInfo.AudioFileId"/>
	public string? AudioFileId { get; set; }

	/// <inheritdoc/>
	public IList<string> ArtistIds { get; set; } = [];

	/// <inheritdoc/>
	public IList<string> GenreIds { get; set; } = [];
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override TrackUpdate GetUpdateFrom(MutableTrack oldState)
	{
		if (Name is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Name)}' to have a value.");

		if (Duration is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Duration)}' to have a value.");

		return new()
		{
			Name = Update.Value(oldState.Name, Name),
			Duration = oldState.Duration is null ? Duration.Value : Update.Value(oldState.Duration, Duration),
			AlbumId = Update.Nullable(oldState.AlbumId, AlbumId),
			AudioFileId = Update.Nullable(oldState.AudioFileId, AudioFileId),
			ArtistIds = Update.List(oldState.ArtistIds, ArtistIds),
			GenreIds = Update.List(oldState.GenreIds, GenreIds),
		};
	}
	#endregion
}
