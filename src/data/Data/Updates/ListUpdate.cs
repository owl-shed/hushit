namespace OwlShed.Hushit.Data.Updates;

/// <summary>
/// 	Represents an update to a list.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
public sealed class ListUpdate<T>
{
	#region Fields
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly List<T> _removed = [], _added = [];

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private List<T>? _set;
	#endregion

	#region Properties
	/// <summary>The items that have been removed from the list.</summary>
	public IReadOnlyList<T> Removed => _removed;

	/// <summary>The items that have been added to the list.</summary>
	public IReadOnlyList<T> Added => _added;

	/// <summary>The items that the list should be set to.</summary>
	/// <remarks>If this is not <see langword="null"/>, then the new list should by fully replaced with this one.</remarks>
	public IReadOnlyList<T>? Set => _set;
	#endregion

	#region Methods
	/// <summary>Adds a new <paramref name="item"/> to the list.</summary>
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

	/// <summary>Removes an <paramref name="item"/> from the list.</summary>
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

	/// <summary>Replaces the entire list with the new items.</summary>
	/// <param name="items">The items to replace the list with.</param>
	public void Replace(params IEnumerable<T> items)
	{
		_removed.Clear();
		_added.Clear();
		_set = [.. items];
	}
	#endregion
}
