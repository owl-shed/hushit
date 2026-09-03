namespace OwlShed.Hushit.Data.AudioFiles;

/// <summary>
/// 	Represents a data model that contains references to audio files.
/// </summary>
public interface IAudioFileReferencesInfo
{
	#region Properties
	/// <summary>The ids of the related audio files.</summary>
	ReadOnlyObservableCollection<string> AudioFileIds { get; }
	#endregion

	#region Methods
	/// <summary>Gets the related audio files.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An asynchronous enumerable of the related audio files.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	IAsyncEnumerable<IAudioFileInfo> GetAudioFilesAsync(CancellationToken cancellation = default);
	#endregion
}
