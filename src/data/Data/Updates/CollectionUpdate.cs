namespace OwlShed.Hushit.Data.Updates;

/// <summary>
/// 	Represents an update to a collection.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public sealed class CollectionUpdate<T>
{
	#region Fields
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly HashSet<T> _removed = [], _added = [];

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private HashSet<T>? _set;
	#endregion

	#region Properties
	/// <summary>The items that have been removed from the collection.</summary>
	public IReadOnlyCollection<T> Removed => _removed;

	/// <summary>The items that have been added to the collection.</summary>
	public IReadOnlyCollection<T> Added => _added;

	/// <summary>The items that the collection should be set to.</summary>
	/// <remarks>If this is not <see langword="null"/>, then the new collection should by fully replaced with this one.</remarks>
	public IReadOnlyCollection<T>? Set => _set;
	#endregion

	#region Methods
	/// <summary>Adds a new <paramref name="item"/> to the collection.</summary>
	/// <param name="item">The item to add.</param>
	public void Add(T item)
	{
		if (_set is not null)
		{
			_set.Add(item);
			return;
		}

		_added.Add(item);
		_removed.Remove(item);
	}

	/// <summary>Removes an <paramref name="item"/> from the collection.</summary>
	/// <param name="item">The item to remove.</param>
	public void Remove(T item)
	{
		if (_set is not null)
		{
			_set.Remove(item);
			return;
		}
		_removed.Remove(item);
		_added.Remove(item);
	}

	/// <summary>Replaces the entire collection with the new items.</summary>
	/// <param name="items">The items to replace the collection with.</param>
	public void Replace(params IReadOnlyCollection<T> items)
	{
		_removed.Clear();
		_added.Clear();
		_set = [.. items];
	}
	#endregion
}
