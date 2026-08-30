namespace OwlShed.Hushit.Data.Tracks.References;

/// <summary>
/// 	Represents a data model that contains references to tracks.
/// </summary>
public interface ITrackReferencesInfo : IDataModel<IMutableTrackReferences, ITrackReferencesUpdate>
{
	#region Properties
	/// <summary>The ids of the related tracks.</summary>
	ReadOnlyObservableCollection<string> TrackIds { get; }
	#endregion

	#region Methods
	/// <summary>Gets the related tracks.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An asynchronous enumerable of the related tracks.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	IAsyncEnumerable<ITrackInfo> GetTracksAsync(CancellationToken cancellation = default);
	#endregion
}
