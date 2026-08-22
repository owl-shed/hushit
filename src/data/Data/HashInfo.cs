namespace OwlShed.Hushit.Data;

/// <summary>
/// 	Represents information about a hash.
/// </summary>
public readonly struct HashInfo
{
	#region Properties
	/// <summary>The kind of the hash.</summary>
	public readonly string Kind { get; }

	/// <summary>The value of the hash.</summary>
	public readonly string Value { get; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="HashInfo"/> instance.</summary>
	/// <param name="kind">The kind of the hash.</param>
	/// <param name="value">The value of the hash.</param>
	/// <exception cref="ArgumentException">
	/// 	Thrown if either the <paramref name="kind"/> or the <paramref name="value"/>
	/// 	are empty or consist only of white-space characters.
	/// </exception>
	public HashInfo(string kind, string value)
	{
		Guard.IsNotNullOrWhiteSpace(kind);
		Guard.IsNotNullOrWhiteSpace(value);

		Kind = kind;
		Value = value;
	}
	#endregion
}
