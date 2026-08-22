namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents an update to the <see cref="ITrackInfo"/>.
/// </summary>
public interface ITrackUpdate
{
	#region Methods
	/// <summary>Sets the new name for the track.</summary>
	/// <param name="name">The new name for the track.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="name"/> was empty.</exception>
	ITrackUpdate WithName(string name);

	/// <summary>Sets the new duration for the track.</summary>
	/// <param name="duration">The new duration for the track.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// 	Thrown if the given <paramref name="duration"/> was less than <see cref="TimeSpan.Zero"/>.
	/// </exception>
	ITrackUpdate WithDuration(TimeSpan duration);

	/// <summary>Sets the id for the new album of the track.</summary>
	/// <param name="id">The id for the new album of the track. A <see langword="null"/> value can be used to remove track from the album.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="id"/> was empty.</exception>
	ITrackUpdate WithAlbum(string? id);

	/// <summary>Sets the id for the new audio file of the track.</summary>
	/// <param name="id">The id for the new audio file of the track. A <see langword="null"/> value can be used to remove track from the audio file.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="id"/> was empty.</exception>
	ITrackUpdate WithAudioFile(string? id);

	/// <summary>Adds a new artist to the track.</summary>
	/// <param name="id">The id of the artist to add to the track.</param>
	/// <returns>The used track update builder.</returns>
	ITrackUpdate AddArtist(string id);

	/// <summary>Removes an artist from the track.</summary>
	/// <param name="id">The id of the artist to remove from the track.</param>
	/// <returns>The used track update builder.</returns>
	ITrackUpdate RemoveArtist(string id);

	/// <summary>Sets the new track artists' ids.</summary>
	/// <param name="artistIds">The ids of the new track artists.</param>
	/// <returns>The used track update builder.</returns>
	ITrackUpdate WithArtists(params IReadOnlyList<string> artistIds);

	/// <summary>Adds a new genre to the track.</summary>
	/// <param name="id">The id of the genre to add to the track.</param>
	/// <returns>The used track update builder.</returns>
	ITrackUpdate AddGenre(string id);

	/// <summary>Removes an genre from the track.</summary>
	/// <param name="id">The id of the genre to remove from the track.</param>
	/// <returns>The used track update builder.</returns>
	ITrackUpdate RemoveGenre(string id);

	/// <summary>Sets the new track genres' ids.</summary>
	/// <param name="genreIds">The ids of the new track genres.</param>
	/// <returns>The used track update builder.</returns>
	ITrackUpdate WithGenres(params IReadOnlyList<string> genreIds);
	#endregion
}
