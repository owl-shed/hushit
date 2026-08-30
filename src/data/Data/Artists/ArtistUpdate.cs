namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents an update to the <see cref="IArtistInfo"/>.
/// </summary>
public sealed class ArtistUpdate
{
	#region Properties
	/// <inheritdoc cref="IArtistInfo.Name"/>
	public ValueUpdate<string> Name { get; set; }

	/// <inheritdoc cref="IArtistInfo.Aliases"/>
	public ListUpdate<string> Aliases { get; set; } = new();

	/// <inheritdoc cref="IGenreReferencesInfo.GenreIds"/>
	public ListUpdate<string> GenreIds { get; set; } = new();

	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	public ListUpdate<string> TrackIds { get; set; } = new();

	/// <inheritdoc cref="IAlbumReferencesInfo.AlbumIds"/>
	public ListUpdate<string> AlbumIds { get; set; } = new();
	#endregion
}
