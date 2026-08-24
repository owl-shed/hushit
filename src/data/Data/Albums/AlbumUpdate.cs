namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents an update to the <see cref="IAlbumInfo"/>.
/// </summary>
public sealed class AlbumUpdate : IArtistReferencesUpdate, ITrackReferencesUpdate, IGenreReferencesUpdate
{
	#region Properties
	/// <inheritdoc cref="IAlbumInfo.Name"/>
	public ValueUpdate<string> Name { get; set; }

	/// <inheritdoc/>
	public ListUpdate<string> ArtistIds { get; set; } = new();

	/// <inheritdoc/>
	public ListUpdate<string> TrackIds { get; set; } = new();

	/// <inheritdoc/>
	public ListUpdate<string> GenreIds { get; set; } = new();
	#endregion
}
