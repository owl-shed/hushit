namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents information about a track.
/// </summary>
public interface ITrackInfo : IDataModel<MutableTrack, TrackUpdate>, IArtistReferencesInfo, IGenreReferencesInfo
{
	#region Properties
	/// <summary>The name of the track.</summary>
	string Name { get; }

	/// <summary>The duration of the track.</summary>
	TimeSpan Duration { get; }

	/// <summary>The id of the album that the track belongs to.</summary>
	string? AlbumId { get; }

	/// <summary>The id of the audio file that the track is linked to.</summary>
	string? AudioFileId { get; }
	#endregion

	#region Methods
	/// <summary>Gets the album that the track belongs to.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The album that the track belongs to, or <see langword="null"/> if the track didn't belong to an album.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<IAlbumInfo?> GetAlbumAsync(CancellationToken cancellation = default);

	/// <summary>Gets the audio file that the track is linked to.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The audio file that the track is linked to, or <see langword="null"/> if the track didn't belong to an audio file.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<IAudioFileInfo?> GetAudioFileAsync(CancellationToken cancellation = default);
	#endregion
}
