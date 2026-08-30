namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents a data model that contains references to genres.
/// </summary>
public interface IGenreReferencesInfo
{
	#region Properties
	/// <summary>The ids of the related genres.</summary>
	ReadOnlyObservableCollection<string> GenreIds { get; }
	#endregion

	#region Methods
	/// <summary>Gets the related genres.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An asynchronous enumerable of the related genres.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	IAsyncEnumerable<IGenreInfo> GetGenresAsync(CancellationToken cancellation = default);
	#endregion
}
