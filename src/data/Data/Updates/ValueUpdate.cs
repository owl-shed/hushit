namespace OwlShed.Hushit.Data.Updates;

/// <summary>
/// 	Represents an update to a single value.
/// </summary>
/// <typeparam name="T">The type of the value being updated.</typeparam>
public readonly struct ValueUpdate<T>
	where T : notnull
{
	#region Properties
	/// <summary>The new value.</summary>
	public T? Value { get; }

	/// <summary>Whether the value has actually changed.</summary>
	[MemberNotNullWhen(true, nameof(Value))]
	public bool HasChanged { get; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="ValueUpdate{T}"/> instance.</summary>
	/// <param name="value">The new value.</param>
	/// <param name="hasChanged">Whether the value has actually changed.</param>
	public ValueUpdate(T? value, bool hasChanged)
	{
		Value = value;
		HasChanged = hasChanged;

		if (hasChanged is true && value is null)
			ThrowHelper.ThrowArgumentException(nameof(value), $"If the value has changed then it should not be null. If you require update tracking for a nullable type then use the {nameof(NullableUpdate<>)} type instead.");
	}

	/// <summary>Creates a new <see cref="ValueUpdate{T}"/> instance.</summary>
	/// <param name="value">The new value.</param>
	public ValueUpdate(T value)
	{
		Value = value;
		HasChanged = true;
	}

	/// <summary>Creates a new <see cref="ValueUpdate{T}"/> instance.</summary>
	public ValueUpdate()
	{
		Value = default;
		HasChanged = false;
	}
	#endregion

	#region Operators
	/// <summary>Creates a new <see cref="ValueUpdate{T}"/> instance directly from the given <paramref name="value"/>.</summary>
	/// <param name="value">The new value.</param>
	public static implicit operator ValueUpdate<T>(T value) => new(value);
	#endregion
}
