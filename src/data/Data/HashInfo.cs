namespace OwlShed.Hushit.Data;

/// <summary>
/// 	Represents information about a hash.
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct HashInfo : IParsable<HashInfo>
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

	#region Methods
	/// <inheritdoc/>
	public override string ToString() => $"{Kind}:{Value}";
	#endregion

	#region Functions
	/// <summary>Tries to parse the given <paramref name="s"/> into a <see cref="HashInfo"/> value.</summary>
	/// <param name="s">The <see langword="string"/> to parse.</param>
	/// <returns>The parsed <see cref="HashInfo"/> value.</returns>
	public static HashInfo Parse(string s)
	{
		if (TryParse(s, null, out HashInfo result) is false)
			ThrowHelper.ThrowArgumentException(nameof(s), "Invalid hash info format.");

		return result;
	}

	/// <inheritdoc/>
	public static HashInfo Parse(string s, IFormatProvider? provider)
	{
		if (TryParse(s, provider, out HashInfo result) is false)
			ThrowHelper.ThrowArgumentException(nameof(s), "Invalid hash info format.");

		return result;
	}

	/// <summary>Tries to parse the given <paramref name="s"/> into a <see cref="HashInfo"/> value.</summary>
	/// <param name="s">The <see langword="string"/> to parse.</param>
	/// <returns>The parsed <see cref="HashInfo"/> value, or <see langword="null"/> if parsing failed.</returns>
	public static HashInfo? TryParse([NotNullWhen(true)] string? s)
	{
		if (TryParse(s, null, out HashInfo result))
			return result;

		return null;
	}

	/// <summary>Tries to parse the given <paramref name="s"/> into a <see cref="HashInfo"/> value.</summary>
	/// <param name="s">The <see langword="string"/> to parse.</param>
	/// <param name="result">The parsed <see cref="HashInfo"/> value.</param>
	/// <returns><see langword="true"/> if the parsing was successful, <see langword="false"/> otherwise.</returns>
	public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out HashInfo result) => TryParse(s, null, out result);

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out HashInfo result)
	{
		if (s is null)
		{
			result = default;
			return false;
		}

		string[] parts = s.Split(':');
		if (parts.Length is 2)
		{
			result = new(parts[0], parts[1]);
			return true;
		}

		result = default;
		return false;
	}
	#endregion

	#region Helpers
	private string DebuggerDisplay() => $"{nameof(HashInfo)} {{ {nameof(Kind)} = ({Kind}), {nameof(Value)} = ({Value}) }}";
	#endregion
}
