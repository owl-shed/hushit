namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents an update to the <see cref="IAlbumInfo"/>.
/// </summary>
public sealed class AlbumUpdate
{
	#region Properties
	/// <inheritdoc cref="IAlbumInfo.Name"/>
	public ValueUpdate<string> Name { get; set; }

	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	public ListUpdate<string> ArtistIds { get; set; } = new();
	#endregion
}
