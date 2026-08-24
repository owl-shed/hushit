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
	#endregion
}
