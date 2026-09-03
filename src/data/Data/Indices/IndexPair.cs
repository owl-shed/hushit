namespace OwlShed.Hushit.Data.Indices;

/// <summary>
/// 	Represents an id/value pair in a data index.
/// </summary>
public readonly struct IndexPair<T>
{
	#region Properties
	/// <summary>The data model id.</summary>
	public readonly string Id { get; }

	/// <summary>The value linked to the <see cref="Id"/>.</summary>
	public readonly T Value { get; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="IndexPair{T}"/> instance.</summary>
	/// <param name="id">The data model id.</param>
	/// <param name="value">The value linked to the <paramref name="id"/>.</param>
	public IndexPair(string id, T value)
	{
		Id = id;
		Value = value;
	}
	#endregion
}
