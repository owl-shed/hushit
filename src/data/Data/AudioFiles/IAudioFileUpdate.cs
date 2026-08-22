namespace OwlShed.Hushit.Data.AudioFiles;

/// <summary>
/// 	Represents an update to the <see cref="IAudioFileInfo"/>.
/// </summary>
public interface IAudioFileUpdate
{
	#region Methods
	/// <summary>Sets the new path for the audio file.</summary>
	/// <param name="path">The new path for the audio file.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given path was empty.</exception>
	IAudioFileUpdate WithPath(string path);

	/// <summary>Sets the new hash for the audio file.</summary>
	/// <param name="hash">The new hash for the audio file.</param>
	/// <returns>The used audio file update builder.</returns>
	IAudioFileUpdate WithHash(HashInfo hash);

	/// <summary>Sets the new name for the track.</summary>
	/// <param name="name">The new name for the track.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	IAudioFileUpdate WithTrackName(string name);

	/// <summary>Sets the new name for the album.</summary>
	/// <param name="name">The new name for the album.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	/// <remarks>A <see langword="null"/> value is allowed in order to remove the album name.</remarks>
	IAudioFileUpdate WithAlbum(string? name);

	/// <summary>Sets the new release date for the track.</summary>
	/// <param name="date">The date on which the track was released.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <remarks>A <see langword="null"/> value is allowed in order to remove the track date.</remarks>
	IAudioFileUpdate WithTrackDate(DateInfo? date);

	/// <summary>Sets the new release date for the album.</summary>
	/// <param name="date">The date on which the album was released.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <remarks>A <see langword="null"/> value is allowed in order to remove the album date.</remarks>
	IAudioFileUpdate WithAlbumDate(DateInfo? date);

	/// <summary>Sets the new artists that made the track.</summary>
	/// <param name="artists">The artists that made the track.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <exception cref="ArgumentException">
	/// 	Thrown if any of the artist names were empty
	/// 	or only consisted of white-space characters.
	/// </exception>
	IAudioFileUpdate WithTrackArtists(params IReadOnlyList<string> artists);

	/// <summary>Sets the new artists that made the album.</summary>
	/// <param name="artists">The artists that made the album.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <exception cref="ArgumentException">
	/// 	Thrown if any of the artist names were empty
	/// 	or only consisted of white-space characters.
	/// </exception>
	IAudioFileUpdate WithAlbumArtists(params IReadOnlyList<string> artists);

	/// <summary>Sets the new genres that the track belongs to.</summary>
	/// <param name="genres">The genres that the track belongs to.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <exception cref="ArgumentException">
	/// 	Thrown if any of the genre names were empty
	/// 	or only consisted of white-space characters.
	/// </exception>
	IAudioFileUpdate WithTrackGenres(params IReadOnlyList<string> genres);

	/// <summary>Sets the new genres that the album belongs to.</summary>
	/// <param name="genres">The genres that the album belongs to.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <exception cref="ArgumentException">
	/// 	Thrown if any of the genre names were empty
	/// 	or only consisted of white-space characters.
	/// </exception>
	IAudioFileUpdate WithAlbumGenres(params IReadOnlyList<string> genres);

	/// <summary>Sets the id for the new track of the audio file.</summary>
	/// <param name="id">The id for the new track of the audio file. A <see langword="null"/> value can be used to remove audio file from the track.</param>
	/// <returns>The used audio file update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="id"/> was empty.</exception>
	IAudioFileUpdate WithTrack(string? id);
	#endregion
}
