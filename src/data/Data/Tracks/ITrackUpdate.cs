namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents an update to the <see cref="ITrackInfo"/>.
/// </summary>
public interface ITrackUpdate : IArtistReferencesUpdate<ITrackUpdate>, IGenreReferencesUpdate<ITrackUpdate>
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
	#endregion
}

/// <summary>
/// 	Contains various extensions related to the <see cref="ITrackUpdate"/>.
/// </summary>
public static class ITrackUpdateExtensions
{
	extension(ITrackUpdate track)
	{
		#region Methods
		/// <summary>Sets the new album of the track.</summary>
		/// <param name="album">The new album of the track. A <see langword="null"/> value can be used to remove track from the album.</param>
		/// <returns>The used track update builder.</returns>
		public ITrackUpdate WithAlbum(IAlbumInfo? album) => track.WithAlbum(album?.Id);

		/// <summary>Sets the new audio file of the track.</summary>
		/// <param name="file">The new audio file of the track. A <see langword="null"/> value can be used to remove track from the audio file.</param>
		/// <returns>The used track update builder.</returns>
		public ITrackUpdate WithAudioFile(IAudioFileInfo? file) => track.WithAudioFile(file?.Id);
		#endregion
	}
}
