namespace OwlShed.Hushit.Data.Updates;

/// <summary>
/// 	Represents a utility class for helping to generate value updates.
/// </summary>
public static class Update
{
	#region Functions
	/// <summary>Creates an update for value of the type <typeparamref name="T"/>.</summary>
	/// <typeparam name="T">The type of the value to update.</typeparam>
	/// <param name="oldValue">The old value.</param>
	/// <param name="newValue">The new value.</param>
	/// <param name="comparer">The comparer to use when determining whether the value has changed.</param>
	/// <returns>A value representing the update.</returns>
	/// <remarks>By default <see cref="EqualityComparer{T}.Default"/> will be used.</remarks>
	public static ValueUpdate<T> Value<T>(T? oldValue, T newValue, IEqualityComparer<T>? comparer = null)
		where T : notnull
	{
		comparer ??= EqualityComparer<T>.Default;

		if (comparer.Equals(oldValue, newValue) is false)
			return new(newValue);

		return default;
	}

	/// <summary>Creates an update for value of the type <typeparamref name="T"/>.</summary>
	/// <typeparam name="T">The type of the value to update.</typeparam>
	/// <param name="oldValue">The old value.</param>
	/// <param name="newValue">The new value.</param>
	/// <param name="comparer">The comparer to use when determining whether the value has changed.</param>
	/// <returns>A value representing the update.</returns>
	/// <remarks>By default <see cref="EqualityComparer{T}.Default"/> will be used.</remarks>
	public static NullableUpdate<T> Nullable<T>(T? oldValue, T? newValue, IEqualityComparer<T>? comparer = null)
		where T : notnull
	{
		comparer ??= EqualityComparer<T>.Default;

		if (comparer.Equals(oldValue, newValue) is false)
			return new(newValue);

		return default;
	}

	/// <summary>Creates an update for value of the type <typeparamref name="T"/>.</summary>
	/// <typeparam name="T">The type of the value to update.</typeparam>
	/// <param name="oldValue">The old value.</param>
	/// <param name="newValue">The new value.</param>
	/// <param name="comparer">The comparer to use when determining whether the value has changed.</param>
	/// <returns>A value representing the update.</returns>
	/// <remarks>By default <see cref="EqualityComparer{T}.Default"/> will be used.</remarks>
	public static NullableUpdate<T?> Nullable<T>(T? oldValue, T? newValue, IEqualityComparer<T?>? comparer = null)
		where T : struct
	{
		comparer ??= EqualityComparer<T?>.Default;

		if (comparer.Equals(oldValue, newValue) is false)
			return new(newValue);

		return default;
	}

	/// <summary>Creates an update for a collection with items of the type <typeparamref name="T"/>.</summary>
	/// <typeparam name="T">The type of the items in the collections.</typeparam>
	/// <param name="oldCollection">The old collection.</param>
	/// <param name="newCollection">The new collection.</param>
	/// <param name="comparer">The comparer to use when determining whether the value has changed.</param>
	/// <returns>A value representing the update.</returns>
	/// <remarks>By default <see cref="EqualityComparer{T}.Default"/> will be used.</remarks>
	public static CollectionUpdate<T> Collection<T>(
		IReadOnlyCollection<T> oldCollection,
		IReadOnlyCollection<T> newCollection,
		IEqualityComparer<T>? comparer = null)
	{
		comparer ??= EqualityComparer<T>.Default;

		bool AreEqual()
		{
			if (oldCollection.Count != newCollection.Count)
				return false;

			if (oldCollection.Count is 0 && newCollection.Count is 0)
				return true;

			foreach (T item in oldCollection.Concat(newCollection).Distinct())
			{
				int oldCount = oldCollection.Count(other => comparer.Equals(other, item));
				int newCount = newCollection.Count(other => comparer.Equals(other, item));

				if (oldCount != newCount)
					return false;
			}

			return false;
		}

		if (AreEqual())
			return new();

		CollectionUpdate<T> update = new();
		update.Replace(newCollection);

		return update;
	}

	/// <summary>Creates an update for a collection with items of the type <typeparamref name="T"/>.</summary>
	/// <typeparam name="T">The type of the items in the list.</typeparam>
	/// <param name="oldList">The old list.</param>
	/// <param name="newList">The new list.</param>
	/// <param name="comparer">The comparer to use when determining whether the value has changed.</param>
	/// <returns>A value representing the update.</returns>
	/// <remarks>By default <see cref="EqualityComparer{T}.Default"/> will be used.</remarks>
	public static ListUpdate<T> List<T>(
		IList<T> oldList,
		IList<T> newList,
		IEqualityComparer<T>? comparer = null)
	{
		comparer ??= EqualityComparer<T>.Default;

		bool AreEqual()
		{
			if (oldList.Count != newList.Count)
				return false;

			if (oldList.Count is 0 && newList.Count is 0)
				return true;

			for (int i = 0; i < oldList.Count; i++)
			{
				T oldItem = oldList[i];
				T newItem = newList[i];

				if (comparer.Equals(oldItem, newItem) is false)
					return false;
			}

			return true;
		}

		if (AreEqual())
			return new();

		ListUpdate<T> update = new();
		update.Replace(newList);

		return update;
	}
	#endregion
}
