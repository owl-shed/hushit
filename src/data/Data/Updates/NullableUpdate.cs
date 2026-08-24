namespace OwlShed.Hushit.Data.Updates;

/// <summary>
/// 	Represents an update to a single nullable value.
/// </summary>
/// <typeparam name="T">The type of the nullable value being updated.</typeparam>
public readonly struct NullableUpdate<T>
{
	#region Properties
	/// <summary>The new value.</summary>
	public T? Value { get; }

	/// <summary>Whether the value has actually changed.</summary>
	public bool HasChanged { get; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="NullableUpdate{T}"/> instance.</summary>
	/// <param name="value">The new value.</param>
	/// <param name="hasChanged">Whether the value has actually changed.</param>
	public NullableUpdate(T? value, bool hasChanged)
	{
		Value = value;
		HasChanged = hasChanged;
	}

	/// <summary>Creates a new <see cref="NullableUpdate{T}"/> instance.</summary>
	/// <param name="value">The new value.</param>
	public NullableUpdate(T? value)
	{
		Value = value;
		HasChanged = true;
	}

	/// <summary>Creates a new <see cref="NullableUpdate{T}"/> instance.</summary>
	public NullableUpdate()
	{
		Value = default;
		HasChanged = false;
	}
	#endregion

	#region Operators
	/// <summary>Creates a new <see cref="NullableUpdate{T}"/> instance directly from the given <paramref name="value"/>.</summary>
	/// <param name="value">The new value.</param>
	public static implicit operator NullableUpdate<T>(T? value) => new(value);
	#endregion
}
