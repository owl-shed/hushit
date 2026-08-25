namespace OwlShed.Hushit.Data.AudioFiles;

/// <summary>
/// 	Represents information about an audio file.
/// </summary>
public interface IAudioFileInfo : IDataModel<MutableAudioFile, AudioFileUpdate>
{
	#region Properties
	/// <summary>The local path to the audio file.</summary>
	string Path { get; }

	/// <summary>The hash of the audio file the last time it was loaded.</summary>
	HashInfo Hash { get; }

	/// <summary>The name of the audio track.</summary>
	/// <remarks>If the value couldn't be loaded from the metadata, then the name of the audio file will be used.</remarks>
	string TrackName { get; }

	/// <summary>The date that the track was released on.</summary>
	DateInfo? TrackDate { get; }

	/// <summary>The name of the album that the audio track belongs to.</summary>
	string? Album { get; }

	/// <summary>The date that the album was released on.</summary>
	DateInfo? AlbumDate { get; }

	/// <summary>The names of the artists that made the track.</summary>
	ReadOnlyObservableCollection<string> TrackArtists { get; }

	/// <summary>The names of the artists that made the album.</summary>
	ReadOnlyObservableCollection<string> AlbumArtists { get; }

	/// <summary>The names of the genres that the track belongs to.</summary>
	ReadOnlyObservableCollection<string> TrackGenres { get; }

	/// <summary>The names of the genres that the album belongs to.</summary>
	ReadOnlyObservableCollection<string> AlbumGenres { get; }

	/// <summary>The id of the track that the audio file is linked to.</summary>
	string? TrackId { get; }
	#endregion

	#region Methods
	/// <summary>Tries to reload the metadata for file, if the file hash changed.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns><see langword="true"/> if any metadata changed, <see langword="false"/> otherwise.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="ArgumentException">Thrown if the audio file no longer existed in the repository.</exception>
	ValueTask<bool> TryReloadAsync(CancellationToken cancellation = default);

	/// <summary>Force reloads the metadata for the file, regardless of the hash check.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns><see langword="true"/> if any metadata changed, <see langword="false"/> otherwise.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="ArgumentException">Thrown if the given audio file no longer existed in the repository.</exception>
	ValueTask<bool> ReloadAsync(CancellationToken cancellation = default);

	/// <summary>Gets the track that the audio file is linked to.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The track that the audio file is linked to, or <see langword="null"/> if the track didn't belong to an audio file.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<ITrackInfo?> GetTrackAsync(CancellationToken cancellation = default);
	#endregion
}
