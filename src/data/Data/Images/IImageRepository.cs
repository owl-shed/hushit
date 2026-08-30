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

	/// <summary>Creates a new data model.</summary>
	/// <param name="path">The path of the image file.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The created data model.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="FileNotFoundException">Thrown if no file exists at the given <paramref name="path"/>.</exception>
	ValueTask<IImageInfo> CreateAsync(string path, CancellationToken cancellation = default);
}
