namespace OwlShed.Hushit.Common;

/// <summary>
/// 	Represents the base class for asynchronous events.
/// </summary>
/// <typeparam name="TCallback">The type of the callback delegate.</typeparam>
/// <remarks>You should not use this base type directly.</remarks>
public abstract class AsyncEventBase<TCallback> : IAsyncEventBase<TCallback>
{
	#region Fields
	private readonly List<TCallback> _callbacks = [];
	private readonly SemaphoreSlim _callbackLock = new(1, 1);
	private readonly SemaphoreSlim _raiseLock = new(1, 1);
	#endregion

	#region Methods
	/// <inheritdoc/>
	public void Subscribe(TCallback callback)
	{
		using (_callbackLock.Lock())
			_callbacks.Add(callback);
	}

	/// <inheritdoc/>
	public void Unsubscribe(TCallback callback)
	{
		using (_callbackLock.Lock())
			_callbacks.Remove(callback);
	}
	#endregion

	#region Helpers
	/// <summary>Prevents the event from being raised until the returned scope is disposed.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A scope which will allow the event to be raised again.</returns>
	protected ValueTask<SemaphoreScope> RaiseLockAsync(CancellationToken cancellation = default)
	{
		return _raiseLock.LockAsync(cancellation);
	}

	/// <summary>Gets the registered callbacks.</summary>
	/// <returns>The list of the registered callbacks, in the order that they were registered in.</returns>
	protected IReadOnlyList<TCallback> GetCallbacks()
	{
		using (_callbackLock.Lock())
			return _callbacks.ToArray();
	}
	#endregion
}

/// <inheritdoc cref="IAsyncEvent"/>
public sealed class AsyncEvent : AsyncEventBase<AsyncEventDelegate>, IAsyncEvent
{
	#region Methods
	/// <summary>
	/// 	Raises the event and calls the registered callbacks sequentially in the order that they were registered in.
	/// </summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async ValueTask RaiseSequentialAsync(CancellationToken cancellation = default)
	{
		foreach (var callback in GetCallbacks())
			await callback.Invoke(cancellation);
	}

	/// <summary>
	/// 	Raises the event and calls the registered callbacks in parallel.
	/// </summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async ValueTask RaiseParallelAsync(CancellationToken cancellation = default)
	{
		Task[] tasks = GetCallbacks()
			.Select(callback => callback.Invoke(cancellation).AsTask())
			.ToArray();

		await Task.WhenAll(tasks);
	}
	#endregion
}

/// <inheritdoc cref="IAsyncEvent{T1}"/>
public sealed class AsyncEvent<T1> : AsyncEventBase<AsyncEventDelegate<T1>>, IAsyncEvent<T1>
{
	#region Methods
	/// <summary>
	/// 	Raises the event and calls the registered callbacks sequentially in the order that they were registered in.
	/// </summary>
	/// <param name="argument1">The 1st argument.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async ValueTask RaiseSequentialAsync(T1 argument1, CancellationToken cancellation = default)
	{
		foreach (var callback in GetCallbacks())
			await callback.Invoke(argument1, cancellation);
	}

	/// <summary>
	/// 	Raises the event and calls the registered callbacks in parallel.
	/// </summary>
	/// <param name="argument1">The 1st argument.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async ValueTask RaiseParallelAsync(T1 argument1, CancellationToken cancellation = default)
	{
		Task[] tasks = GetCallbacks()
			.Select(callback => callback.Invoke(argument1, cancellation).AsTask())
			.ToArray();

		await Task.WhenAll(tasks);
	}
	#endregion
}

/// <inheritdoc cref="IAsyncEvent{T1, T2}"/>
public sealed class AsyncEvent<T1, T2> : AsyncEventBase<AsyncEventDelegate<T1, T2>>, IAsyncEvent<T1, T2>
{
	#region Methods
	/// <summary>
	/// 	Raises the event and calls the registered callbacks sequentially in the order that they were registered in.
	/// </summary>
	/// <param name="argument1">The 1st argument.</param>
	/// <param name="argument2">The 2nd argument.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async ValueTask RaiseSequentialAsync(T1 argument1, T2 argument2, CancellationToken cancellation = default)
	{
		foreach (var callback in GetCallbacks())
			await callback.Invoke(argument1, argument2, cancellation);
	}

	/// <summary>
	/// 	Raises the event and calls the registered callbacks in parallel.
	/// </summary>
	/// <param name="argument1">The 1st argument.</param>
	/// <param name="argument2">The 2nd argument.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async ValueTask RaiseParallelAsync(T1 argument1, T2 argument2, CancellationToken cancellation = default)
	{
		Task[] tasks = GetCallbacks()
			.Select(callback => callback.Invoke(argument1, argument2, cancellation).AsTask())
			.ToArray();

		await Task.WhenAll(tasks);
	}
	#endregion
}

/// <inheritdoc cref="IAsyncEvent{T1, T2, T3}"/>
public sealed class AsyncEvent<T1, T2, T3> : AsyncEventBase<AsyncEventDelegate<T1, T2, T3>>, IAsyncEvent<T1, T2, T3>
{
	#region Methods
	/// <summary>
	/// 	Raises the event and calls the registered callbacks sequentially in the order that they were registered in.
	/// </summary>
	/// <param name="argument1">The 1st argument.</param>
	/// <param name="argument2">The 2nd argument.</param>
	/// <param name="argument3">The 3rd argument.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async ValueTask RaiseSequentialAsync(T1 argument1, T2 argument2, T3 argument3, CancellationToken cancellation = default)
	{
		foreach (var callback in GetCallbacks())
			await callback.Invoke(argument1, argument2, argument3, cancellation);
	}

	/// <summary>
	/// 	Raises the event and calls the registered callbacks in parallel.
	/// </summary>
	/// <param name="argument1">The 1st argument.</param>
	/// <param name="argument2">The 2nd argument.</param>
	/// <param name="argument3">The 3rd argument.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async ValueTask RaiseParallelAsync(T1 argument1, T2 argument2, T3 argument3, CancellationToken cancellation = default)
	{
		Task[] tasks = GetCallbacks()
			.Select(callback => callback.Invoke(argument1, argument2, argument3, cancellation).AsTask())
			.ToArray();

		await Task.WhenAll(tasks);
	}
	#endregion
}
