namespace OwlShed.Hushit.Data.Repositories;

internal abstract class DataRepositoryBase<TModel, TMutable, TUpdate, TTypedModel> : IDataRepository<TModel, TMutable, TUpdate>
	where TModel : notnull, IDataModel<TMutable, TUpdate>
	where TMutable : notnull, IMutableDataModel<TMutable, TUpdate>, new()
	where TUpdate : notnull
	where TTypedModel : class, TModel
{
	#region Fields
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly AsyncEvent<ModelUpdateInfo<TModel, TMutable, TUpdate>> _modelUpdated = new();

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly AsyncEvent<TModel> _modelAdded = new(), _modelRemoved = new();

	private readonly ReaderWriterLockSlim _cacheLock = new();
	private readonly Dictionary<string, WeakReference<TTypedModel>> _cache = [];
	#endregion

	#region Properties
	/// <summary>The facade to the Hushit data.</summary>
	protected IHushitData Data { get; }

	/// <inheritdoc/>
	public IAsyncEvent<ModelUpdateInfo<TModel, TMutable, TUpdate>> ModelUpdated => _modelUpdated;

	/// <inheritdoc/>
	public IAsyncEvent<TModel> ModelAdded => _modelAdded;

	/// <inheritdoc/>
	public IAsyncEvent<TModel> ModelRemoved => _modelRemoved;
	#endregion

	#region Constructors
	protected DataRepositoryBase(IHushitData data)
	{
		Data = data;
	}
	#endregion

	#region Methods
	/// <inheritdoc/>
	public virtual void Initialise() { }

	/// <inheritdoc/>
	public async ValueTask<TModel> CreateAsync(Func<TMutable, CancellationToken, ValueTask> callback, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string id = CreateNewId();
		return await CreateAsync(id, callback, cancellation).ConfigureAwait(false);
	}
	protected async ValueTask<TModel> CreateAsync(string id, Func<TMutable, CancellationToken, ValueTask> callback, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		TMutable mutable = new();
		await callback.Invoke(mutable, cancellation).ConfigureAwait(false);

		TTypedModel model = Create(id, mutable);
		Cache(model);

		await PersistAsync(model, cancellation).ConfigureAwait(false);

		await OnCreatedAsync(model, cancellation).ConfigureAwait(false);
		await _modelAdded.RaiseSequentialAsync(model, cancellation).ConfigureAwait(false);

		TMutable oldState = new();
		TMutable newState = model.ToMutable();
		TUpdate update = newState.GetUpdateFrom(oldState);

		await _modelUpdated.RaiseSequentialAsync(new(ModelUpdateKind.Added, model, oldState, newState, update), cancellation).ConfigureAwait(false);

		return model;
	}

	/// <inheritdoc/>
	public async ValueTask<TModel> CreateAsync(Action<TMutable> callback, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string id = CreateNewId();
		return await CreateAsync(id, callback, cancellation).ConfigureAwait(false);
	}
	protected async ValueTask<TModel> CreateAsync(string id, Action<TMutable> callback, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		TMutable mutable = new();
		callback.Invoke(mutable);
		cancellation.ThrowIfCancellationRequested();

		TTypedModel model = Create(id, mutable);
		Cache(model);

		await PersistAsync(model, cancellation).ConfigureAwait(false);

		await OnCreatedAsync(model, cancellation).ConfigureAwait(false);
		await _modelAdded.RaiseSequentialAsync(model, cancellation).ConfigureAwait(false);

		TMutable oldState = new();
		TMutable newState = model.ToMutable();
		TUpdate update = newState.GetUpdateFrom(oldState);

		await _modelUpdated.RaiseSequentialAsync(new(ModelUpdateKind.Added, model, oldState, newState, update), cancellation).ConfigureAwait(false);

		return model;
	}
	protected virtual ValueTask OnCreatedAsync(TTypedModel model, CancellationToken cancellation = default) => default;

	/// <inheritdoc/>
	public async IAsyncEnumerable<TModel> GetAllAsync([EnumeratorCancellation] CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		await foreach (string id in GetPersistedIdsAsync(cancellation).WithCancellation(cancellation).ConfigureAwait(false))
		{
			TModel? model = await TryGetAsync(id, cancellation).ConfigureAwait(false);

			if (model is not null)
				yield return model;
		}
	}

	/// <inheritdoc/>
	public async ValueTask<TModel?> TryGetAsync(string id, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		return await TryGetCoreAsync(id, cancellation).ConfigureAwait(false);
	}
	protected async ValueTask<TTypedModel?> TryGetCoreAsync(string? id, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		if (id is null)
			return null;

		if (TryGetCached(id, out TTypedModel? model))
			return model;

		TTypedModel? typed = await TryLoadPersistedAsync(id, cancellation).ConfigureAwait(false);

		if (typed is not null)
			Cache(typed);

		return typed;
	}

	/// <inheritdoc/>
	public async ValueTask<bool> UpdateAsync(string id, Action<TMutable> callback, CancellationToken cancellation = default)
	{
		TTypedModel? model = await TryGetCoreAsync(id, cancellation).ConfigureAwait(false);
		if (model?.Exists is not true)
			ThrowHelper.ThrowArgumentException(nameof(id), $"The data model with the given id ({id}) no longer existed.");

		TMutable oldState = model.ToMutable();
		TMutable newState = model.ToMutable();

		callback.Invoke(newState);
		TUpdate update = newState.GetUpdateFrom(oldState);

		CopyState(model, oldState, newState, update);
		await PersistAsync(model, cancellation).ConfigureAwait(false);

		await OnUpdateAsync(model, oldState, newState, update, cancellation).ConfigureAwait(false);
		await _modelUpdated.RaiseSequentialAsync(new(ModelUpdateKind.Changed, model, oldState, newState, update), cancellation).ConfigureAwait(false);

		return true;
	}

	/// <inheritdoc/>
	public async ValueTask<bool> UpdateAsync(string id, Func<TMutable, CancellationToken, ValueTask> callback, CancellationToken cancellation = default)
	{
		TTypedModel? model = await TryGetCoreAsync(id, cancellation).ConfigureAwait(false);
		if (model?.Exists is not true)
			ThrowHelper.ThrowArgumentException(nameof(id), $"The data model with the given id ({id}) no longer existed.");

		TMutable oldState = model.ToMutable();
		TMutable newState = model.ToMutable();

		await callback.Invoke(newState, cancellation).ConfigureAwait(false);
		TUpdate update = newState.GetUpdateFrom(oldState);

		CopyState(model, oldState, newState, update);
		await PersistAsync(model, cancellation).ConfigureAwait(false);

		await OnUpdateAsync(model, oldState, newState, update, cancellation).ConfigureAwait(false);
		await _modelUpdated.RaiseSequentialAsync(new(ModelUpdateKind.Changed, model, oldState, newState, update), cancellation).ConfigureAwait(false);

		return true;
	}
	protected virtual ValueTask OnUpdateAsync(TTypedModel model, TMutable oldState, TMutable newState, TUpdate update, CancellationToken cancellation = default) => default;

	/// <inheritdoc/>
	public async ValueTask<bool> RemoveAsync(string id, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		TTypedModel? model = await TryGetCoreAsync(id, cancellation).ConfigureAwait(false);

		if (model is null)
			return false;

		MarkAsDeleted(model);
		RemoveFromCache(id);

		await StopPersistingAsync(id, cancellation);

		await OnRemovedAsync(model, cancellation).ConfigureAwait(false);
		await _modelRemoved.RaiseSequentialAsync(model, cancellation).ConfigureAwait(false);

		TMutable oldState = model.ToMutable();
		TMutable newState = new();
		TUpdate update = newState.GetUpdateFrom(oldState);

		await _modelUpdated.RaiseSequentialAsync(new(ModelUpdateKind.Removed, model, oldState, newState, update), cancellation).ConfigureAwait(false);

		return true;
	}
	protected virtual ValueTask OnRemovedAsync(TTypedModel model, CancellationToken cancellation = default) => default;
	#endregion

	#region Cache methods
	protected bool TryGetCached(string id, [NotNullWhen(true)] out TTypedModel? model)
	{
		using (_cacheLock.ReadLock())
		{
			if (_cache.TryGetValue(id, out WeakReference<TTypedModel>? weakRef) && weakRef.TryGetTarget(out model))
				return true;
		}

		using (_cacheLock.UpgradeableReadLock())
		{
			if (_cache.TryGetValue(id, out WeakReference<TTypedModel>? weakRef))
			{
				if (weakRef.TryGetTarget(out model))
					return true;

				using (_cacheLock.WriteLock())
					_cache.Remove(id);
			}
		}

		model = default;
		return false;
	}
	protected void Cache(TTypedModel model)
	{
		using (_cacheLock.ReadLock())
		{
			if (_cache.TryGetValue(model.Id, out WeakReference<TTypedModel>? weakRef) && weakRef.TryGetTarget(out _))
				return;
		}

		using (_cacheLock.UpgradeableReadLock())
		{
			if (_cache.TryGetValue(model.Id, out WeakReference<TTypedModel>? weakRef))
			{
				if (weakRef.TryGetTarget(out _))
					return;

				using (_cacheLock.WriteLock())
				{
					weakRef.SetTarget(model);
					return;
				}
			}

			using (_cacheLock.WriteLock())
				_cache.Add(model.Id, new(model));
		}
	}
	protected void RemoveFromCache(string id)
	{
		using (_cacheLock.ReadLock())
		{
			if (_cache.ContainsKey(id) is false)
				return;
		}

		using (_cacheLock.UpgradeableReadLock())
		{
			if (_cache.ContainsKey(id) is false)
				return;

			using (_cacheLock.WriteLock())
				_cache.Remove(id);
		}
	}
	#endregion

	#region Persist methods
	protected abstract ValueTask PersistAsync(TTypedModel model, CancellationToken cancellation = default);
	protected abstract IAsyncEnumerable<string> GetPersistedIdsAsync(CancellationToken cancellation = default);
	protected abstract ValueTask<TTypedModel?> TryLoadPersistedAsync(string id, CancellationToken cancellation = default);
	protected abstract ValueTask StopPersistingAsync(string id, CancellationToken cancellation = default);
	#endregion

	#region Helpers
	protected virtual string CreateNewId() => Guid.NewGuid().ToString("N");
	protected abstract TTypedModel Create(string id, TMutable initialState);
	protected abstract void MarkAsDeleted(TTypedModel model);
	protected abstract void CopyState(TTypedModel model, TMutable oldState, TMutable newState, TUpdate update);
	#endregion
}
