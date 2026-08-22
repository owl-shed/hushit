namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents an update to the <see cref="IArtistInfo"/>.
/// </summary>
public interface IArtistUpdate
{
	#region Methods
	/// <summary>Sets the new name for the artist.</summary>
	/// <param name="name">The new name for the artist.</param>
	/// <returns>The used artist update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	IArtistUpdate WithName(string name);

	/// <summary>Adds a new alias for the artist.</summary>
	/// <param name="alias">The new alias.</param>
	/// <returns>The used artist update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given alias was empty.</exception>
	IArtistUpdate AddAlias(string alias);

	/// <summary>Removes an alias from the artist.</summary>
	/// <param name="alias">The alias to remove.</param>
	/// <returns>The used artist update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given alias was empty.</exception>
	IArtistUpdate RemoveAlias(string alias);

	/// <summary>Sets the new aliases for the artist.</summary>
	/// <param name="aliases">The new aliases to set for the artist.</param>
	/// <returns>The used artist update builder.</returns>
	/// <remarks>This will override all of the previous aliases.</remarks>
	/// <exception cref="ArgumentException">Thrown if any of the given aliases was empty.</exception>
	IArtistUpdate WithAliases(params IReadOnlyList<string> aliases);

	/// <summary>Adds a new album to the artist.</summary>
	/// <param name="id">The id of the album to add to the artist.</param>
	/// <returns>The used artist update builder.</returns>
	IArtistUpdate AddAlbum(string id);

	/// <summary>Removes an album from the artist.</summary>
	/// <param name="id">The id of the album to remove from the artist.</param>
	/// <returns>The used artist update builder.</returns>
	IArtistUpdate RemoveAlbum(string id);

	/// <summary>Sets the new artist albums' ids.</summary>
	/// <param name="albumIds">The ids of the new artist albums.</param>
	/// <returns>The used artist update builder.</returns>
	IArtistUpdate WithAlbums(params IReadOnlyList<string> albumIds);

	/// <summary>Adds a new track to the artist.</summary>
	/// <param name="id">The id of the track to add to the artist.</param>
	/// <returns>The used artist update builder.</returns>
	IArtistUpdate AddTrack(string id);

	/// <summary>Removes an track from the artist.</summary>
	/// <param name="id">The id of the track to remove from the artist.</param>
	/// <returns>The used artist update builder.</returns>
	IArtistUpdate RemoveTrack(string id);

	/// <summary>Sets the new artist tracks' ids.</summary>
	/// <param name="trackIds">The ids of the new artist tracks.</param>
	/// <returns>The used artist update builder.</returns>
	IArtistUpdate WithTracks(params IReadOnlyList<string> trackIds);

	/// <summary>Adds a new genre to the artist.</summary>
	/// <param name="id">The id of the genre to add to the artist.</param>
	/// <returns>The used artist update builder.</returns>
	IArtistUpdate AddGenre(string id);

	/// <summary>Removes an genre from the artist.</summary>
	/// <param name="id">The id of the genre to remove from the artist.</param>
	/// <returns>The used artist update builder.</returns>
	IArtistUpdate RemoveGenre(string id);

	/// <summary>Sets the new artist genres' ids.</summary>
	/// <param name="genreIds">The ids of the new artist genres.</param>
	/// <returns>The used artist update builder.</returns>
	IArtistUpdate WithGenres(params IReadOnlyList<string> genreIds);
	#endregion
}
