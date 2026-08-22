namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents a repository for genres.
/// </summary>
public interface IGenreRepository : IDataRepository<IGenreInfo, IGenreUpdate>
{
	#region Methods
	/// <inheritdoc cref="IDataRepository{TModel, TUpdate}.CreateAsync(Action{TUpdate}, CancellationToken)"/>
	/// <remarks>
	/// 	Every genre requires:
	/// 	<list type="bullet">
	/// 		<item>A name.</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<IGenreInfo> CreateAsync(Action<IGenreUpdate> callback, CancellationToken cancellation = default);
	#endregion
}
