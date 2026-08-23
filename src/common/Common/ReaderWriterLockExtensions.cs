namespace OwlShed.Hushit.Common;

/// <summary>
/// 	Represents a scope which will exit a read lock when disposed.
/// </summary>
/// <param name="lock">The managed lock.</param>
public readonly ref struct ReaderWriterReadLock(ReaderWriterLockSlim @lock) : IDisposable
{
	#region Methods
	/// <inheritdoc/>
	public void Dispose() => @lock.ExitReadLock();
	#endregion
}

/// <summary>
/// 	Represents a scope which will exit a write lock when disposed.
/// </summary>
/// <param name="lock">The managed lock.</param>
public readonly ref struct ReaderWriterWriteLock(ReaderWriterLockSlim @lock) : IDisposable
{
	#region Methods
	/// <inheritdoc/>
	public void Dispose() => @lock.ExitWriteLock();
	#endregion
}

/// <summary>
/// 	Represents a scope which will exit an upgradeable read lock when disposed.
/// </summary>
/// <param name="lock">The managed lock.</param>
public readonly ref struct ReaderWriterUpgradeableReadLock(ReaderWriterLockSlim @lock) : IDisposable
{
	#region Methods
	/// <inheritdoc/>
	public void Dispose() => @lock.ExitUpgradeableReadLock();
	#endregion
}

/// <summary>
/// 	Contains various extensions related to the <see cref="ReaderWriterLockSlim"/>.
/// </summary>
public static class ReaderWriterLockExtensions
{
	extension(ReaderWriterLockSlim @lock)
	{
		#region Methods
		/// <summary>Enters a read lock.</summary>
		/// <returns>A scope which will exit the read lock when disposed.</returns>
		public ReaderWriterReadLock ReadLock()
		{
			@lock.EnterReadLock();
			return new(@lock);
		}

		/// <summary>Enters a write lock.</summary>
		/// <returns>A scope which will exit the write lock when disposed.</returns>
		public ReaderWriterWriteLock WriteLock()
		{
			@lock.EnterWriteLock();
			return new(@lock);
		}

		/// <summary>Enters an upgradeable read lock.</summary>
		/// <returns>A scope which will exit the upgradeable read lock when disposed.</returns>
		public ReaderWriterUpgradeableReadLock UpgradeableReadLock()
		{
			@lock.EnterUpgradeableReadLock();
			return new(@lock);
		}
		#endregion
	}
}
