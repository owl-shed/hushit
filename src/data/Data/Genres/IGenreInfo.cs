namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents information about a genre.
/// </summary>
public interface IGenreInfo : IDataModel<MutableGenre, GenreUpdate>, IArtistReferencesInfo, IAlbumReferencesInfo, ITrackReferencesInfo
{
	#region Properties
	/// <summary>The name of the genre.</summary>
	string Name { get; }
	#endregion
}
