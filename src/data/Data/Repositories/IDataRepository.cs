namespace OwlShed.Hushit.Data.Repositories;

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
	#region Events
	/// <summary>An event that is raised when a new model is added.</summary>
	IAsyncEvent<TModel> ModelAdded { get; }

	/// <summary>An event that is raised when a new model is removed.</summary>
	IAsyncEvent<TModel> ModelRemoved { get; }
	#endregion

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
/// <typeparam name="TMutable">The type that represents the mutable <typeparamref name="TModel"/>.</typeparam>
public interface IDataRepository<TModel, TMutable> : IDataRepository<TModel>
	where TModel : notnull, IDataModel
	where TMutable : notnull, IMutableDataModel
{
	#region Methods
	/// <summary>Creates a new data model.</summary>
	/// <param name="callback">A callback that can be used to set the initial information about the data model.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The created data model.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the <paramref name="callback"/> didn't provide the required information.</exception>
	ValueTask<TModel> CreateAsync(Action<TMutable> callback, CancellationToken cancellation = default);

	/// <summary>Updates the given <paramref name="id"/>.</summary>
	/// <param name="id">The id of the data model to update.</param>
	/// <param name="callback">A callback that can be used to update the data model.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns><see langword="true"/> if any changes were made to the data <paramref name="id"/>, <see langword="false"/> otherwise.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	/// <exception cref="ArgumentException">Thrown if the data model with the given <paramref name="id"/> no longer existed in the repository.</exception>
	ValueTask<bool> UpdateAsync(string id, Action<TMutable> callback, CancellationToken cancellation = default);
	#endregion
}

/// <summary>
/// 	Represents the arguments for a model update event.
/// </summary>
/// <typeparam name="TModel">The type of the model that was updated.</typeparam>
/// <typeparam name="TMutable">The type for the mutable version of the <typeparamref name="TModel"/>.</typeparam>
/// <typeparam name="TUpdate">The type that represents the update between two <typeparamref name="TMutable"/> instances.</typeparam>
public readonly struct ModelUpdateInfo<TModel, TMutable, TUpdate>
	where TModel : notnull, IDataModel<TMutable, TUpdate>
	where TMutable : notnull, IMutableDataModel<TMutable, TUpdate>
	where TUpdate : notnull
{
	#region Properties
	/// <summary>The model that was updated.</summary>
	public TModel Model { get; }

	/// <summary>The old state of the model.</summary>
	public TMutable Old { get; }

	/// <summary>The new state of the model.</summary>
	public TMutable New { get; }

	/// <summary>The update from the old state, to the new state of the model.</summary>
	public TUpdate Update { get; }
	#endregion

	#region Constructors
	/// <summary>Creates a new instance of the <see cref="ModelUpdateInfo{TModel, TMutable, TUpdate}"/>.</summary>
	/// <param name="model">The model that was updated.</param>
	/// <param name="oldState">The old state of the model.</param>
	/// <param name="newState">The new state of the model.</param>
	/// <param name="update">The update from the old state, to the new state of the model.</param>
	public ModelUpdateInfo(TModel model, TMutable oldState, TMutable newState, TUpdate update)
	{
		Model = model;
		Old = oldState;
		New = newState;
		Update = update;
	}
	#endregion
}

/// <summary>
/// 	Represents a data model repository.
/// </summary>
/// <typeparam name="TModel">The type of the models that the repository manages.</typeparam>
/// <typeparam name="TMutable">The type that represents the mutable <typeparamref name="TModel"/>.</typeparam>
/// <typeparam name="TUpdate">The type that represents an update between two <typeparamref name="TMutable"/> instances.</typeparam>
public interface IDataRepository<TModel, TMutable, TUpdate> : IDataRepository<TModel, TMutable>
	where TModel : notnull, IDataModel<TMutable, TUpdate>
	where TMutable : notnull, IMutableDataModel<TMutable, TUpdate>
	where TUpdate : notnull
{
	#region Events
	/// <summary>An event that is raised when an existing model is updated.</summary>
	IAsyncEvent<ModelUpdateInfo<TModel, TMutable, TUpdate>> ModelUpdated { get; }
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

	extension<TModel, TMutable>(IDataRepository<TModel, TMutable> repository)
		where TModel : notnull, IDataModel<TMutable>
		where TMutable : notnull, IMutableDataModel
	{
		#region Methods
		/// <summary>Updates the data model with the given <paramref name="model"/>..</summary>
		/// <param name="model">The data model to update.</param>
		/// <param name="callback">A callback that can be used to update the data model.</param>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns><see langword="true"/> if any changes were made to the data model, <see langword="false"/> otherwise.</returns>
		/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
		/// <exception cref="ArgumentException">Thrown if the data model no longer existed in the repository.</exception>
		public async ValueTask<bool> UpdateAsync(TModel model, Action<TMutable> callback, CancellationToken cancellation = default)
		{
			cancellation.ThrowIfCancellationRequested();
			return await repository.UpdateAsync(model.Id, callback, cancellation).ConfigureAwait(false);
		}
		#endregion
	}
}
