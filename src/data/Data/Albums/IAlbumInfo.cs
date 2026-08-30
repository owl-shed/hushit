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

	#region Mutable methods
	/// <inheritdoc cref="IDataModel{T}.ToMutable"/>
	new MutableAlbum ToMutable();
	MutableAlbum IDataModel<MutableAlbum>.ToMutable() => ToMutable();
	IMutableArtistReferences IDataModel<IMutableArtistReferences>.ToMutable() => ToMutable();
	IMutableGenreReferences IDataModel<IMutableGenreReferences>.ToMutable() => ToMutable();
	IMutableTrackReferences IDataModel<IMutableTrackReferences>.ToMutable() => ToMutable();
	#endregion
}
