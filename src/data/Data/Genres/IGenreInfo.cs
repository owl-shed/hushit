namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents information about a genre.
/// </summary>
public interface IGenreInfo : IDataModel<GenreUpdate, MutableGenre>, IArtistReferencesInfo, IAlbumReferencesInfo, ITrackReferencesInfo
{
	#region Properties
	/// <summary>The name of the genre.</summary>
	string Name { get; }
	#endregion
}
