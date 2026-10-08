namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents a repository for genres.
/// </summary>
public interface IGenreRepository : IDataRepository<IGenreInfo, MutableGenre, GenreUpdate>
{
	#region Methods
	/// <inheritdoc cref="IDataRepository{TModel, TMutable}.CreateAsync(Action{TMutable}, CancellationToken)"/>
	/// <remarks>
	/// 	Every genre requires:
	/// 	<list type="bullet">
	/// 		<item>A name.</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<IGenreInfo> CreateAsync(Action<MutableGenre> callback, CancellationToken cancellation = default);

	/// <summary>Atomically gets or creates a new genre with the given <paramref name="name"/>.</summary>
	/// <param name="name">The name of the genre.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The found or created genre.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<IGenreInfo> GetOrCreateAsync(string name, CancellationToken cancellation = default);
	#endregion
}
