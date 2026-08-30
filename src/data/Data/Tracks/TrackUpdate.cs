namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents an update to the <see cref="ITrackInfo"/>.
/// </summary>
public sealed class TrackUpdate
{
	#region Properties
	/// <inheritdoc cref="ITrackInfo.Name"/>
	public ValueUpdate<string> Name { get; set; }

	/// <inheritdoc cref="ITrackInfo.Duration"/>
	public ValueUpdate<TimeSpan> Duration { get; set; }

	/// <inheritdoc cref="ITrackInfo.AlbumId"/>
	public NullableUpdate<string> AlbumId { get; set; }

	/// <inheritdoc cref="ITrackInfo.AudioFileId"/>
	public NullableUpdate<string> AudioFileId { get; set; }

	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	public ListUpdate<string> ArtistIds { get; set; } = new();

	/// <inheritdoc cref="IGenreReferencesInfo.GenreIds"/>
	public ListUpdate<string> GenreIds { get; set; } = new();
	#endregion
}
