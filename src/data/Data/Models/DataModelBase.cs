namespace OwlShed.Hushit.Data.Models;

/// <summary>
/// 	Represents the base class for a data model.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
/// <typeparam name="TMutable">The type for the mutable version of the data model.</typeparam>
/// <typeparam name="TUpdate">The type that represents an update between two <typeparamref name="TMutable"/> instances.</typeparam>
internal abstract class DataModelBase<TModel, TMutable, TUpdate> : ObservableBase, IDataModel<TMutable, TUpdate>
	where TModel : notnull, IDataModel<TMutable, TUpdate>
	where TMutable : notnull, IMutableDataModel<TMutable, TUpdate>
	where TUpdate : notnull
{
	#region Properties
	protected ReaderWriterLockSlim Lock { get; } = new(LockRecursionPolicy.SupportsRecursion);

	/// <summary>The facade to the Hushit data.</summary>
	protected IHushitData Data { get; }
	protected abstract IDataRepository<TModel, TMutable, TUpdate> Repository { get; }

	/// <inheritdoc/>
	public string Id { get; }

	/// <inheritdoc/>
	/// <remarks>This value should only be updated from the data repository that owns the data model.</remarks>
	public bool Exists { get => Read(ref field); internal set => TrySet(ref field, value); } = true;
	#endregion

	#region Constructors
	protected DataModelBase(IHushitData data, string id)
	{
		Data = data;
		Id = id;
	}
	#endregion

	#region Methods
	/// <inheritdoc/>
	public async ValueTask<bool> UpdateAsync(Action<TMutable> callback, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		return await Repository.UpdateAsync(Id, callback, cancellation).ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public async ValueTask<bool> UpdateAsync(Func<TMutable, CancellationToken, ValueTask> callback, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		return await Repository.UpdateAsync(Id, callback, cancellation).ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public async ValueTask<bool> RemoveAsync(CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		return await Repository.RemoveAsync(Id, cancellation).ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public abstract TMutable ToMutable();

	/// <summary>Override to copy the given <paramref name="state"/> to the current instance.</summary>
	/// <param name="state">The new state to copy.</param>
	/// <remarks>This value should only be called from the data repository that owns the data model.</remarks>
	internal abstract void CopyState(TMutable state);

	protected T Read<T>(ref T field)
	{
		using (Lock.ReadLock())
			return field;
	}

	/// <inheritdoc/>
	protected override bool TrySet<T>(ref T field, T newValue, [CallerMemberName] string? property = null)
	{
		using (Lock.WriteLock())
			return base.TrySet(ref field, newValue, property);
	}
	#endregion
}
