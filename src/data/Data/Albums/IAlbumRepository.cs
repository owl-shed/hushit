namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents a repository for albums.
/// </summary>
public interface IAlbumRepository : IDataRepository<IAlbumInfo, IAlbumUpdate>
{
	#region Methods
	/// <inheritdoc cref="IDataRepository{TModel, TUpdate}.CreateAsync(Action{TUpdate}, CancellationToken)"/>
	/// <remarks>
	/// 	Every album requires:
	/// 	<list type="bullet">
	/// 		<item>A name.</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<IAlbumInfo> CreateAsync(Action<IAlbumUpdate> callback, CancellationToken cancellation = default);
	#endregion
}
