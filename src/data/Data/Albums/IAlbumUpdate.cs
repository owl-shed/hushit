namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents an update to the <see cref="IAlbumInfo"/>.
/// </summary>
public interface IAlbumUpdate
{
	#region Methods
	/// <summary>Sets the new name for the album.</summary>
	/// <param name="name">The new name for the album.</param>
	/// <returns>The used album update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	IAlbumUpdate WithName(string name);

	/// <summary>Adds a new artist to the album.</summary>
	/// <param name="id">The id of the artist to add to the album.</param>
	/// <returns>The used album update builder.</returns>
	IAlbumUpdate AddArtist(string id);

	/// <summary>Removes an artist from the album.</summary>
	/// <param name="id">The id of the artist to remove from the album.</param>
	/// <returns>The used album update builder.</returns>
	IAlbumUpdate RemoveArtist(string id);

	/// <summary>Sets the new album artists' ids.</summary>
	/// <param name="artistIds">The ids of the new album artists.</param>
	/// <returns>The used album update builder.</returns>
	IAlbumUpdate WithArtists(params IReadOnlyList<string> artistIds);

	/// <summary>Adds a new genre to the album.</summary>
	/// <param name="id">The id of the genre to add to the album.</param>
	/// <returns>The used album update builder.</returns>
	IAlbumUpdate AddGenre(string id);

	/// <summary>Removes an genre from the album.</summary>
	/// <param name="id">The id of the genre to remove from the album.</param>
	/// <returns>The used album update builder.</returns>
	IAlbumUpdate RemoveGenre(string id);

	/// <summary>Sets the new album genres' ids.</summary>
	/// <param name="genreIds">The ids of the new album genres.</param>
	/// <returns>The used album update builder.</returns>
	IAlbumUpdate WithGenres(params IReadOnlyList<string> genreIds);
	#endregion
}
