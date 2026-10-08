using System.Collections.ObjectModel;

namespace OwlShed.Hushit.Common;

/// <summary>
///   Contains various extensions related to the <see cref="ObservableCollection{T}"/>.
/// </summary>
public static class ObservableCollectionExtensions
{
	extension<T>(ObservableCollection<T> collection)
	{
		#region Methods
		/// <summary>Replaces the items in the collection with the given <paramref name="newItems"/>.</summary>
		/// <param name="newItems">The new items to replace the collection with.</param>
		/// <param name="comparer">The comparer to use for determining equality.</param>
		/// <remarks>By default the <see cref="EqualityComparer{T}.Default"/> will be used.</remarks>
		public void Replace(IEnumerable<T> newItems, IEqualityComparer<T>? comparer = null)
		{
			comparer ??= EqualityComparer<T>.Default;

			int index = -1;
			foreach (T item in newItems)
			{
				index++;

				if (index < collection.Count)
				{
					if (comparer.Equals(collection[index], item))
						continue;

					collection[index] = item;
					continue;
				}

				collection.Add(item);
			}

			while (index < collection.Count - 1)
				collection.RemoveAt(collection.Count - 1);
		}
		#endregion
	}
}
