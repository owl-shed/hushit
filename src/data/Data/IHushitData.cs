namespace OwlShed.Hushit.Data;

/// <summary>
/// 	Represents the settings to use for importing new data.
/// </summary>
public sealed class ImportSettings
{
	#region Properties
	/// <summary>The degree of concurrency to use when importing the audio files.</summary>
	public int Concurrency { get; set; } = Environment.ProcessorCount;

	/// <summary>Whether to forcefully reload the metadata if the audio file was already imported before.</summary>
	public bool Force { get; set; } = false;
	#endregion
}

/// <summary>
/// 	Represents an access point to all of the data that Hushit stores and manages.
/// </summary>
public interface IHushitData
{
	#region Properties
	/// <summary>The data repository for audio file information.</summary>
	IAudioFileRepository AudioFiles { get; }

	/// <summary>The data repository for artist information.</summary>
	IArtistRepository Artists { get; }

	/// <summary>The data repository for album information.</summary>
	IAlbumRepository Albums { get; }

	/// <summary>The data repository for track information.</summary>
	ITrackRepository Tracks { get; }

	/// <summary>The data repository for genre information.</summary>
	IGenreRepository Genres { get; }

	/// <summary>The data repository for image file information.</summary>
	IImageRepository Images { get; }
	#endregion

	#region Methods
	/// <summary>Imports (or reloads) the given <paramref name="files"/> into the repositories.</summary>
	/// <param name="files">The audio files to import.</param>
	/// <param name="settings">The settings to use when importing the <paramref name="files"/>.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask ImportAsync(IReadOnlyCollection<string> files, ImportSettings settings, CancellationToken cancellation = default);

	/// <summary>Reloads the imported audio files.</summary>
	/// <param name="settings">The settings to use when reloading the audio files.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask ReloadAsync(ImportSettings settings, CancellationToken cancellation = default);
	#endregion
}

/// <summary>
/// 	Contains various extensions related to the <see cref="IHushitData"/>.
/// </summary>
public static class IHushitDataExtensions
{
	extension(IHushitData data)
	{
		#region Methods
		/// <summary>Imports (or reloads) the given <paramref name="files"/> into the repositories.</summary>
		/// <param name="files">The audio files to import.</param>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
		public async ValueTask ImportAsync(IReadOnlyCollection<string> files, CancellationToken cancellation = default)
		{
			ImportSettings settings = new();
			await data.ImportAsync(files, settings, cancellation).ConfigureAwait(false);
		}

		/// <summary>Reloads the imported audio files.</summary>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
		public async ValueTask ReloadAsync(CancellationToken cancellation = default)
		{
			ImportSettings settings = new();
			await data.ReloadAsync(settings, cancellation).ConfigureAwait(false);
		}
		#endregion
	}
}
