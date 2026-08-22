namespace OwlShed.Hushit.Data;

/// <summary>
/// 	Represents an access point to all of the data that Hushit stores and manages.
/// </summary>
public interface IHushitData
{
	#region Properties
	/// <summary>The data repository for audio file information.</summary>
	IAudioFileRepository AudioFiles { get; }

	/// <summary>The data repository for artist information.</summary>
	IArtistRepository Artists { get; }

	/// <summary>The data repository for album information.</summary>
	IAlbumRepository Albums { get; }

	/// <summary>The data repository for track information.</summary>
	ITrackRepository Tracks { get; }

	/// <summary>The data repository for genre information.</summary>
	IGenreRepository Genres { get; }
	#endregion
}
