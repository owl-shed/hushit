namespace OwlShed.Hushit.Common;

/// <summary>
/// 	Represents a scope which will release a semaphore when disposed.
/// </summary>
/// <param name="semaphore">The managed semaphore.</param>
/// <param name="count">The amount of times to release the semaphore.</param>
public readonly struct SemaphoreScope(SemaphoreSlim semaphore, int count = 1) : IDisposable
{
	#region Methods
	/// <inheritdoc/>
	public readonly void Dispose() => semaphore.Release(count);
	#endregion
}

/// <summary>
///   Contains various extensions related to the <see cref="SemaphoreSlim"/>.
/// </summary>
public static class SemaphoreSlimExtensions
{
	extension(SemaphoreSlim semaphore)
	{
		#region Methods
		/// <summary>Waits to acquire the semaphore.</summary>
		/// <returns>A scope which will release the semaphore when disposed.</returns>
		public SemaphoreScope Lock()
		{
			semaphore.Wait();
			return new(semaphore);
		}
		/// <summary>Waits to acquire the semaphore.</summary>
		/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
		/// <returns>A scope which will release the semaphore when disposed.</returns>
		public async ValueTask<SemaphoreScope> LockAsync(CancellationToken cancellation = default)
		{
			await semaphore.WaitAsync(cancellation);
			return new(semaphore);
		}
		#endregion
	}
}
