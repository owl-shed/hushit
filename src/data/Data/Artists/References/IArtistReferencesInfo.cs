namespace OwlShed.Hushit.Data.Artists.References;

/// <summary>
/// 	Represents a data model that contains references to artists.
/// </summary>
public interface IArtistReferencesInfo : IDataModel<IArtistReferencesUpdate>
{
	#region Properties
	/// <summary>The ids of the related artists.</summary>
	ReadOnlyObservableCollection<string> ArtistIds { get; }
	#endregion

	#region Methods
	/// <summary>Gets the related artists.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An asynchronous enumerable of the related artists.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	IAsyncEnumerable<IArtistInfo> GetArtistsAsync(CancellationToken cancellation = default);
	#endregion
}
