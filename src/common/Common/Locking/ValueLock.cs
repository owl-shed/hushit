namespace OwlShed.Hushit.Common.Locking;

/// <summary>
/// 	Represents a custom lock for types of the given <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the values that locks will be made on.</typeparam>
public sealed class ValueLock<T>
	where T : notnull
{
	#region Nested types
	private sealed class Scope(ValueLock<T> valueLock, T value) : IAsyncDisposable
	{
		#region Methods
		public async ValueTask DisposeAsync() => await valueLock.EndLockAsync(value);
		#endregion
	}
	#endregion

	#region Fields
	private readonly Dictionary<T, SemaphoreSlim> _locks = [];
	private readonly SemaphoreSlim _lock = new(1, 1);
	#endregion

	#region Methods
	/// <summary>Creates a new lock for the given <paramref name="value"/>.</summary>
	/// <param name="value">The value to create the lock for.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A value which will release the lock for the given <paramref name="value"/> when disposed.</returns>
	public async ValueTask<IAsyncDisposable> LockAsync(T value, CancellationToken cancellation = default)
	{
		bool created = false;
		SemaphoreSlim? semaphore;
		using (await _lock.LockAsync(cancellation).ConfigureAwait(false))
		{
			if (_locks.TryGetValue(value, out semaphore) is false)
			{
				created = true;
				semaphore = new(1, 1);
				_locks.Add(value, semaphore);
			}
		}

		try
		{
			await semaphore.WaitAsync(cancellation).ConfigureAwait(false);
			return new Scope(this, value);
		}
		catch (OperationCanceledException)
		{
			if (created)
			{
				using (await semaphore.LockAsync().ConfigureAwait(false))
					_locks.Remove(value);
			}

			throw;
		}
	}
	private async ValueTask EndLockAsync(T value)
	{
		using (await _lock.LockAsync().ConfigureAwait(false))
		{
			if (_locks.Remove(value, out SemaphoreSlim? semaphore))
				semaphore.Release();
		}
	}
	#endregion
}
