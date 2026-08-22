namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents information about a genre.
/// </summary>
public interface IGenreInfo : IDataModel<IGenreUpdate>
{
	#region Properties
	/// <summary>The name of the genre.</summary>
	string Name { get; }

	/// <summary>The ids of the artists that are involved in this genre.</summary>
	ReadOnlyObservableCollection<string> ArtistIds { get; }

	/// <summary>The ids of the albums that are involved in this genre.</summary>
	ReadOnlyObservableCollection<string> AlbumIds { get; }

	/// <summary>The ids of the tracks that are involved in this genre.</summary>
	ReadOnlyObservableCollection<string> TrackIds { get; }
	#endregion
}
