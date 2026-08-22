namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents an update for the <see cref="IGenreInfo"/>.
/// </summary>
public interface IGenreUpdate
{
	#region Methods
	/// <summary>Sets the new name for the genre.</summary>
	/// <param name="name">The new name for the genre.</param>
	/// <returns>The used genre update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	IGenreUpdate WithName(string name);

	/// <summary>Adds a new artist to the genre.</summary>
	/// <param name="id">The id of the artist to add to the genre.</param>
	/// <returns>The used genre update builder.</returns>
	IGenreUpdate AddArtist(string id);

	/// <summary>Removes an artist from the genre.</summary>
	/// <param name="id">The id of the artist to remove from the genre.</param>
	/// <returns>The used genre update builder.</returns>
	IGenreUpdate RemoveArtist(string id);

	/// <summary>Sets the new genre artists' ids.</summary>
	/// <param name="artistIds">The ids of the new genre artists.</param>
	/// <returns>The used genre update builder.</returns>
	IGenreUpdate WithArtists(params IReadOnlyList<string> artistIds);

	/// <summary>Adds a new album to the genre.</summary>
	/// <param name="id">The id of the album to add to the genre.</param>
	/// <returns>The used genre update builder.</returns>
	IGenreUpdate AddAlbum(string id);

	/// <summary>Removes an album from the genre.</summary>
	/// <param name="id">The id of the album to remove from the genre.</param>
	/// <returns>The used genre update builder.</returns>
	IGenreUpdate RemoveAlbum(string id);

	/// <summary>Sets the new genre albums' ids.</summary>
	/// <param name="albumIds">The ids of the new genre albums.</param>
	/// <returns>The used genre update builder.</returns>
	IGenreUpdate WithAlbums(params IReadOnlyList<string> albumIds);

	/// <summary>Adds a new track to the genre.</summary>
	/// <param name="id">The id of the track to add to the genre.</param>
	/// <returns>The used genre update builder.</returns>
	IGenreUpdate AddTrack(string id);

	/// <summary>Removes an track from the genre.</summary>
	/// <param name="id">The id of the track to remove from the genre.</param>
	/// <returns>The used genre update builder.</returns>
	IGenreUpdate RemoveTrack(string id);

	/// <summary>Sets the new genre tracks' ids.</summary>
	/// <param name="trackIds">The ids of the new genre tracks.</param>
	/// <returns>The used genre update builder.</returns>
	IGenreUpdate WithTracks(params IReadOnlyList<string> trackIds);
	#endregion
}
