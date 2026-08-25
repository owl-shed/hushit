namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents information about an album.
/// </summary>
public interface IAlbumInfo : IDataModel<MutableAlbum, AlbumUpdate>, IArtistReferencesInfo, ITrackReferencesInfo, IGenreReferencesInfo
{
	#region Properties
	/// <summary>The name of the album.</summary>
	string Name { get; }
	#endregion
}
