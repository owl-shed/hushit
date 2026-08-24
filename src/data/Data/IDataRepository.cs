namespace OwlShed.Hushit.Data;

/// <summary>
/// 	Represents a data model repository.
/// </summary>
public interface IDataRepository
{
	#region Methods
	/// <summary>Gets all of the data models stored in the repository.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An asynchronous enumerable for all of the stored data models.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	IAsyncEnumerable<IDataModel> GetAllAsync(CancellationToken cancellation = default);

	/// <summary>Tries to get the data model with the given <paramref name="id"/>.</summary>
	/// <param name="id">The id of the data model to try and get.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>
	/// 	The data model found for the given <paramref name="id"/>, or <see langword="null"/>
	/// 	if no data model with the given <paramref name="id"/> existed in the repository.
	/// </returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<IDataModel?> TryGetAsync(string id, CancellationToken cancellation = default);

	/// <summary>Tries to remove the data model with the given <paramref name="id"/> from the repository.</summary>
	/// <param name="id">The id of the data model to try and remove.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns><see langword="true"/> if the data model was removed, <see langword="false"/> if it didn't before this call.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<bool> RemoveAsync(string id, CancellationToken cancellation = default);
	#endregion
}

/// <summary>
/// 	Represents a data model repository.
/// </summary>
/// <typeparam name="TModel">The type of the models that the repository manages.</typeparam>
public interface IDataRepository<TModel> : IDataRepository
	where TModel : notnull, IDataModel
{
	#region Methods
	/// <summary>Gets all of the data models stored in the repository.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>An asynchronous enumerable for all of the stored data models.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	new IAsyncEnumerable<TModel> GetAllAsync(CancellationToken cancellation = default);
	async IAsyncEnumerable<IDataModel> IDataRepository.GetAllAsync([EnumeratorCancellation] CancellationToken cancellation)
	{
		cancellation.ThrowIfCancellationRequested();

		await foreach (TModel typed in GetAllAsync(cancellation).WithCancellation(cancellation).ConfigureAwait(false))
			yield return typed;
	}

	/// <summary>Tries to get the data model with the given <paramref name="id"/>.</summary>
	/// <param name="id">The id of the data model to try and get.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>
	/// 	The data model found for the given <paramref name="id"/>, or <see langword="null"/>
	/// 	if no data model with the given <paramref name="id"/> existed in the repository.
	/// </returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	new ValueTask<TModel?> TryGetAsync(string id, CancellationToken cancellation = default);
	async ValueTask<IDataModel?> IDataRepository.TryGetAsync(string id, CancellationToken cancellation)
	{
		cancellation.ThrowIfCancellationRequested();

		TModel? model = await TryGetAsync(id, cancellation).ConfigureAwait(false);
		return model;
	}
	#endregion
}

/// <summary>
/// 	Represents a data model repository.
/// </summary>
/// <typeparam name="TModel">The type of the models that the repository manages.</typeparam>
/// <typeparam name="TUpdate">The type that represents the model update builder.</typeparam>
public interface IDataRepository<TModel, TUpdate> : IDataRepository<TModel>
	where TModel : notnull, IDataModel<TUpdate>
	where TUpdate : notnull
{
	#region Methods
	/// <summary>Creates a new data model.</summary>
	/// <param name="callback">A callback that can be used to set the initial information about the data model.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The created data model.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the <paramref name="callback"/> didn't provide the required information.</exception>
	ValueTask<TModel> CreateAsync(Action<TUpdate> callback, CancellationToken cancellation = default);

	/// <summary>Updates the given <paramref name="id"/>.</summary>
	/// <param name="id">The id of the data model to update.</param>
	/// <param name="callback">A callback that can be used to update the data model.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns><see langword="true"/> if any changes were made to the data <paramref name="id"/>, <see langword="false"/> otherwise.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="ArgumentException">Thrown if the data model with the given <paramref name="id"/> no longer existed in the repository.</exception>
	ValueTask<bool> UpdateAsync(string id, Action<TUpdate> callback, CancellationToken cancellation = default);
	#endregion
}

/// <summary>
/// 	Contains various extensions related to the <see cref="IDataRepository"/>.
/// </summary>
public static class IDataRepositoryExtensions
{
	extension(IDataRepository repository)
	{
		#region Methods
		/// <summary>Gets the data model with the given <paramref name="id"/>.</summary>
		/// <param name="id">The id of the data model to get.</param>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns>The data model for the given <paramref name="id"/>.</returns>
		/// <exception cref="ArgumentException">Thrown if no data model with the given <paramref name="id"/> existed.</exception>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
		public async ValueTask<IDataModel> GetAsync(string id, CancellationToken cancellation = default)
		{
			cancellation.ThrowIfCancellationRequested();

			IDataModel? model = await repository.TryGetAsync(id, cancellation).ConfigureAwait(false);
			if (model is null)
				ThrowHelper.ThrowArgumentException(nameof(id), $"No data model with the given id ({id}) existed in the repository.");

			return model;
		}
		#endregion
	}

	extension<TModel>(IDataRepository<TModel> repository)
		where TModel : notnull, IDataModel
	{
		#region Methods
		/// <summary>Gets the data model with the given <paramref name="id"/>.</summary>
		/// <param name="id">The id of the data model to get.</param>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns>The data model for the given <paramref name="id"/>.</returns>
		/// <exception cref="ArgumentException">Thrown if no data model with the given <paramref name="id"/> existed.</exception>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
		public async ValueTask<TModel> GetAsync(string id, CancellationToken cancellation = default)
		{
			cancellation.ThrowIfCancellationRequested();

			TModel? model = await repository.TryGetAsync(id, cancellation).ConfigureAwait(false);
			if (model is null)
				ThrowHelper.ThrowArgumentException(nameof(id), $"No data model with the given id ({id}) existed in the repository.");

			return model;
		}

		/// <summary>Tries to remove the given data <paramref name="model"/> from the repository.</summary>
		/// <param name="model">The data model to try and remove.</param>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns><see langword="true"/> if the data model was removed, <see langword="false"/> if it didn't exist before this call.</returns>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
		public async ValueTask<bool> RemoveAsync(TModel model, CancellationToken cancellation = default)
		{
			cancellation.ThrowIfCancellationRequested();
			return await repository.RemoveAsync(model.Id, cancellation).ConfigureAwait(false);
		}
		#endregion
	}

	extension<TModel, TUpdate>(IDataRepository<TModel, TUpdate> repository)
		where TModel : notnull, IDataModel<TUpdate>
		where TUpdate : notnull
	{
		#region Methods
		/// <summary>Updates the data model with the given <paramref name="model"/>..</summary>
		/// <param name="model">The data model to update.</param>
		/// <param name="callback">A callback that can be used to update the data model.</param>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns><see langword="true"/> if any changes were made to the data model, <see langword="false"/> otherwise.</returns>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
		/// <exception cref="ArgumentException">Thrown if the data model no longer existed in the repository.</exception>
		public async ValueTask<bool> UpdateAsync(TModel model, Action<TUpdate> callback, CancellationToken cancellation = default)
		{
			cancellation.ThrowIfCancellationRequested();
			return await repository.UpdateAsync(model.Id, callback, cancellation).ConfigureAwait(false);
		}
		#endregion
	}
}
