namespace OwlShed.Hushit.Data.AudioFiles;

/// <summary>
/// 	Represents a repository for audio files.
/// </summary>
public interface IAudioFileRepository : IDataRepository<IAudioFileInfo, MutableAudioFile, AudioFileUpdate>
{
	#region Methods
	/// <inheritdoc cref="IDataRepository{TModel, TMutable}.CreateAsync(Action{TMutable}, CancellationToken)"/>
	/// <remarks>
	/// 	Every audio file requires:
	/// 	<list type="bullet">
	/// 		<item>A path.</item>
	/// 		<item>A hash.</item>
	/// 		<item>A track name (if missing from metadata, the name of the file can be used).</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<IAudioFileInfo> CreateAsync(Action<MutableAudioFile> callback, CancellationToken cancellation = default);

	/// <summary>Creates a new data model.</summary>
	/// <param name="path">The path of the audio file.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The created data model.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="FileNotFoundException">Thrown if no file exists at the given <paramref name="path"/>.</exception>
	ValueTask<IAudioFileInfo> CreateAsync(string path, CancellationToken cancellation = default);

	/// <summary>Tries to reload the metadata for the given <paramref name="file"/>, if the file hash changed.</summary>
	/// <param name="file">The audio file to reload the metadata for.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns><see langword="true"/> if any metadata changed, <see langword="false"/> otherwise.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="ArgumentException">Thrown if the given audio <paramref name="file"/> no longer existed in the repository.</exception>
	ValueTask<bool> TryReloadAsync(IAudioFileInfo file, CancellationToken cancellation = default);

	/// <summary>Force reloads the metadata for the given <paramref name="file"/>, regardless of the hash check.</summary>
	/// <param name="file">The audio file to reload the metadata for.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns><see langword="true"/> if any metadata changed, <see langword="false"/> otherwise.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="ArgumentException">Thrown if the given audio <paramref name="file"/> no longer existed in the repository.</exception>
	ValueTask<bool> ReloadAsync(IAudioFileInfo file, CancellationToken cancellation = default);
	#endregion
}

/// <summary>
/// 	Contains various extension methods related to the <see cref="IAudioFileRepository"/>.
/// </summary>
public static class IAudioFileRepositoryExtensions
{
	extension(IAudioFileRepository repository)
	{
		#region Methods
		/// <summary>Tries to reloads the metadata for the audio file with the given <paramref name="id"/>, if the file hash changed.</summary>
		/// <param name="id">The id of the audio file to reload the metadata for.</param>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns><see langword="true"/> if any metadata changed, <see langword="false"/> otherwise.</returns>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
		/// <exception cref="ArgumentException">Thrown if an audio file with the given <paramref name="id"/> didn't exist in the repository.</exception>
		public async ValueTask<bool> TryReloadAsync(string id, CancellationToken cancellation = default)
		{
			cancellation.ThrowIfCancellationRequested();

			IAudioFileInfo? file = await repository.TryGetAsync(id, cancellation).ConfigureAwait(false);
			if (file is null)
				ThrowHelper.ThrowArgumentException(nameof(id), $"No data file with the id ({id}) existed in the repository.");

			return await repository.TryReloadAsync(file, cancellation).ConfigureAwait(false);
		}

		/// <summary>Force reloads the metadata for the audio file with the given <paramref name="id"/>, regardless of the hash check.</summary>
		/// <param name="id">The id of the audio file to reload the metadata for.</param>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns><see langword="true"/> if any metadata changed, <see langword="false"/> otherwise.</returns>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
		/// <exception cref="ArgumentException">Thrown if an audio file with the given <paramref name="id"/> didn't exist in the repository.</exception>
		public async ValueTask<bool> ReloadAsync(string id, CancellationToken cancellation = default)
		{
			cancellation.ThrowIfCancellationRequested();

			IAudioFileInfo? file = await repository.TryGetAsync(id, cancellation).ConfigureAwait(false);
			if (file is null)
				ThrowHelper.ThrowArgumentException(nameof(id), $"No data file with the id ({id}) existed in the repository.");

			return await repository.ReloadAsync(file, cancellation).ConfigureAwait(false);
		}
		#endregion
	}
}
