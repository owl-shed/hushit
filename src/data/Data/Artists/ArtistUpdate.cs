namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents an update to the <see cref="IArtistInfo"/>.
/// </summary>
public sealed class ArtistUpdate : IAlbumReferencesUpdate, ITrackReferencesUpdate, IGenreReferencesUpdate
{
	#region Properties
	/// <inheritdoc cref="IArtistInfo.Name"/>
	public ValueUpdate<string> Name { get; set; }

	/// <inheritdoc cref="IArtistInfo.Aliases"/>
	public ListUpdate<string> Aliases { get; set; } = new();

	/// <inheritdoc/>
	public ListUpdate<string> GenreIds { get; set; } = new();

	/// <inheritdoc/>
	public ListUpdate<string> TrackIds { get; set; } = new();

	/// <inheritdoc/>
	public ListUpdate<string> AlbumIds { get; set; } = new();
	#endregion
}
