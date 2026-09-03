namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents a repository for albums.
/// </summary>
public interface IAlbumRepository : IDataRepository<IAlbumInfo, MutableAlbum, AlbumUpdate>
{
	#region Methods
	/// <inheritdoc cref="IDataRepository{TModel, TMutable}.CreateAsync(Action{TMutable}, CancellationToken)"/>
	/// <remarks>
	/// 	Every album requires:
	/// 	<list type="bullet">
	/// 		<item>A name.</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<IAlbumInfo> CreateAsync(Action<MutableAlbum> callback, CancellationToken cancellation = default);

	/// <summary>Atomically gets or creates a new album with the given <paramref name="name"/> and <paramref name="artistIds"/>.</summary>
	/// <param name="name">The name of the album.</param>
	/// <param name="artistIds">The artists that "own" the album.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The found or created artist.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<IAlbumInfo> GetOrCreateAsync(string name, IReadOnlyCollection<string> artistIds, CancellationToken cancellation = default);
	#endregion
}
