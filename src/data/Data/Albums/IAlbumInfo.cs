namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents information about an album.
/// </summary>
public interface IAlbumInfo : IDataModel<IAlbumUpdate>
{
	#region Properties
	/// <summary>The name of the album.</summary>
	string Name { get; }

	/// <summary>The ids of the album's artists.</summary>
	ReadOnlyObservableCollection<string> ArtistIds { get; }

	/// <summary>The ids of the album's tracks.</summary>
	ReadOnlyObservableCollection<string> TrackIds { get; }

	/// <summary>The ids of the album's genres.</summary>
	ReadOnlyObservableCollection<string> GenreIds { get; }
	#endregion
}
