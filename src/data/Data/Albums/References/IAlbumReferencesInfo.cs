namespace OwlShed.Hushit.Data.Albums.References;

/// <summary>
/// 	Represents a data model that contains references to albums.
/// </summary>
public interface IAlbumReferencesInfo : IDataModel<IAlbumReferencesUpdate>
{
	#region Properties
	/// <summary>The ids of the related albums.</summary>
	ReadOnlyObservableCollection<string> AlbumIds { get; }
	#endregion

	#region Methods
	/// <summary>Gets the related albums.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An asynchronous enumerable of the related albums.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	IAsyncEnumerable<IAlbumInfo> GetAlbumsAsync(CancellationToken cancellation = default);
	#endregion
}
