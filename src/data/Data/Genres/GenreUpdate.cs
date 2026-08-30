namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents an update for the <see cref="IGenreInfo"/>.
/// </summary>
public sealed class GenreUpdate
{
	#region Properties
	/// <inheritdoc cref="IGenreInfo.Name"/>
	public ValueUpdate<string> Name { get; set; }

	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	public ListUpdate<string> ArtistIds { get; set; } = new();

	/// <inheritdoc cref="IAlbumReferencesInfo.AlbumIds"/>
	public ListUpdate<string> AlbumIds { get; set; } = new();

	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	public ListUpdate<string> TrackIds { get; set; } = new();
	#endregion
}
