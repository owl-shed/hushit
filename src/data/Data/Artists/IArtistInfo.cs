namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents information about an artist.
/// </summary>
public interface IArtistInfo : IDataModel<IArtistUpdate>
{
	#region Properties
	/// <summary>The primary name of the artist.</summary>
	string Name { get; }

	/// <summary>Any aliases that the artist may have.</summary>
	ReadOnlyObservableCollection<string> Aliases { get; }

	/// <summary>The ids of the albums that the artist is involved in.</summary>
	ReadOnlyObservableCollection<string> AlbumIds { get; }

	/// <summary>The ids of the tracks that the artist is involved in.</summary>
	ReadOnlyObservableCollection<string> TrackIds { get; }

	/// <summary>The ids of the genres that the artist is involved in.</summary>
	ReadOnlyObservableCollection<string> GenreIds { get; }
	#endregion
}
