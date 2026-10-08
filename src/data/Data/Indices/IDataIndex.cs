namespace OwlShed.Hushit.Data.Indices;

/// <summary>
/// 	Represents an index for data models.
/// </summary>
/// <typeparam name="T">The type of the lookup value in the index.</typeparam>
public interface IDataIndex<T>
{
	#region Methods
	/// <summary>Associates the given data model <paramref name="id"/> with the given <paramref name="key"/>.</summary>
	/// <param name="id">The id of the data model to associated with the given <paramref name="key"/>.</param>
	/// <param name="key">The lookup key.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	ValueTask SetAsync(string id, T key, CancellationToken cancellation = default);

	/// <summary>Removed the data model <paramref name="id"/> from the index.</summary>
	/// <param name="id">The id of the data model to remove from the index.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	ValueTask RemoveAsync(string id, CancellationToken cancellation = default);

	/// <summary>Gets the data model ids for the given index <paramref name="key"/>.</summary>
	/// <param name="key">They key to look up in the index.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The list of the associated data model ids.</returns>
	/// <remarks>There should only be one associated data model id, however multiple may be returned to account for data errors.</remarks>
	ValueTask<IReadOnlyList<string>> GetAsync(T key, CancellationToken cancellation = default);

	/// <summary>Gets a data model id for the given <paramref name="key"/>, or associates the given <paramref name="id"/> with the <paramref name="key"/>.</summary>
	/// <param name="key">The key to get the data model id for.</param>
	/// <param name="id">The id to associate with the given <paramref name="key"/> in case it is missing.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>Either the existing data model id for the given <paramref name="key"/>, or the newly associated given <paramref name="id"/>.</returns>
	ValueTask<string> GetOrAddAsync(T key, string id, CancellationToken cancellation = default);

	/// <summary>Gets all of the information stored in the index.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An async enumerable of all of the information stored in the index.</returns>
	IAsyncEnumerable<IndexPair<T>> GetAllAsync(CancellationToken cancellation = default);
	#endregion
}
