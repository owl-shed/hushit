namespace OwlShed.Hushit.Data.Images;

/// <summary>
/// 	Represents a repository for image files.
/// </summary>
public interface IImageRepository : IDataRepository<IImageInfo, MutableImage, ImageUpdate>
{
	/// <inheritdoc cref="IDataRepository{TModel, TMutable}.CreateAsync(Action{TMutable}, CancellationToken)"/>
	/// <remarks>
	/// 	Every image file requires:
	/// 	<list type="bullet">
	/// 		<item>A path.</item>
	/// 		<item>A hash.</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<IImageInfo> CreateAsync(Action<MutableImage> callback, CancellationToken cancellation = default);

	/// <summary>Gets or creates an image based on the image at the given <paramref name="path"/>.</summary>
	/// <param name="path">The path of the image file.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The found or created image.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="FileNotFoundException">Thrown if no file exists at the given <paramref name="path"/>.</exception>
	ValueTask<IImageInfo> GetOrCreateAsync(string path, CancellationToken cancellation = default);
}
