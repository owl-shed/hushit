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
	#endregion
}
