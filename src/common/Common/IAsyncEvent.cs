namespace OwlShed.Hushit.Common;

/// <summary>
/// 	Represents the base interface for asynchronous events.
/// </summary>
/// <typeparam name="TCallback">The type of the callback delegate.</typeparam>
/// <remarks>You should not implement this interface directly.</remarks>
public interface IAsyncEventBase<in TCallback>
{
	#region Methods
	/// <summary>Subscribes to the event.</summary>
	/// <param name="callback">The asynchronous callback to invoke when the event is raised.</param>
	void Subscribe(TCallback callback);

	/// <summary>Unsubscribes from the event.</summary>
	/// <param name="callback">The asynchronous callback to stop from being invoked when the event is raised.</param>
	void Unsubscribe(TCallback callback);
	#endregion
}

/// <summary>
/// 	Represents a callback delegate for an <see cref="IAsyncEvent"/>.
/// </summary>
/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
public delegate ValueTask AsyncEventDelegate(CancellationToken cancellation = default);

/// <summary>
///   Represents an asynchronous event that doesn't take any arguments.
/// </summary>
public interface IAsyncEvent : IAsyncEventBase<AsyncEventDelegate> { }

/// <summary>
/// 	Represents a callback delegate for an <see cref="IAsyncEvent{T1}"/>.
/// </summary>
/// <param name="argument1">The 1st argument.</param>
/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
public delegate ValueTask AsyncEventDelegate<in T1>(T1 argument1, CancellationToken cancellation = default);

/// <summary>
///   Represents an asynchronous event that takes some arguments.
/// </summary>
/// <typeparam name="T1">The type of the 1st argument.</typeparam>
public interface IAsyncEvent<T1> : IAsyncEventBase<AsyncEventDelegate<T1>> { }

/// <summary>
/// 	Represents a callback delegate for an <see cref="IAsyncEvent{T1, T2}"/>.
/// </summary>
/// <param name="argument1">The 1st argument.</param>
/// <param name="argument2">The 2nd argument.</param>
/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
public delegate ValueTask AsyncEventDelegate<in T1, in T2>(T1 argument1, T2 argument2, CancellationToken cancellation = default);

/// <summary>
///   Represents an asynchronous event that takes some arguments.
/// </summary>
/// <typeparam name="T1">The type of the 1st argument.</typeparam>
/// <typeparam name="T2">The type of the 2nd argument.</typeparam>
public interface IAsyncEvent<T1, T2> : IAsyncEventBase<AsyncEventDelegate<T1, T2>> { }

/// <summary>
/// 	Represents a callback delegate for an <see cref="IAsyncEvent{T1, T2}"/>.
/// </summary>
/// <param name="argument1">The 1st argument.</param>
/// <param name="argument2">The 2nd argument.</param>
/// <param name="argument3">The 3rd argument.</param>
/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
public delegate ValueTask AsyncEventDelegate<in T1, in T2, in T3>(T1 argument1, T2 argument2, T3 argument3, CancellationToken cancellation = default);

/// <summary>
///   Represents an asynchronous event that takes some arguments.
/// </summary>
/// <typeparam name="T1">The type of the 1st argument.</typeparam>
/// <typeparam name="T2">The type of the 2nd argument.</typeparam>
/// <typeparam name="T3">The type of the 3rd argument.</typeparam>
public interface IAsyncEvent<T1, T2, T3> : IAsyncEventBase<AsyncEventDelegate<T1, T2, T3>> { }
