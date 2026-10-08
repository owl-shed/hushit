namespace OwlShed.Hushit.Data;

/// <summary>
/// 	Represents potentially partial information about a date.
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct DateInfo : IParsable<DateInfo>
{
	#region Properties
	/// <summary>The year.</summary>
	public readonly int? Year { get; }

	/// <summary>The month in the year.</summary>
	public readonly int? Month { get; }

	/// <summary>The day in the month.</summary>
	public readonly int? Day { get; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="DateInfo"/> instance.</summary>
	/// <param name="year">The year.</param>
	/// <param name="month">The month in the year.</param>
	/// <param name="day">The day in the month.</param>
	public DateInfo(int? year, int? month, int? day)
	{
		Year = year;
		Month = month;
		Day = day;
	}
	#endregion

	#region Methods
	/// <inheritdoc/>
	public override string ToString()
	{
		if (Year is null)
			return "?";

		if (Month is null)
			return $"{Year}";

		if (Month is null)
			return $"{Year}-{Month}";

		return $"{Year}-{Month}-{Day}";
	}
	#endregion

	#region Functions
	/// <summary>Tries to parse the given <paramref name="s"/> into a <see cref="DateInfo"/> value.</summary>
	/// <param name="s">The <see langword="string"/> to parse.</param>
	/// <returns>The parsed <see cref="DateInfo"/> value.</returns>
	public static DateInfo Parse(string s)
	{
		if (TryParse(s, null, out DateInfo result) is false)
			ThrowHelper.ThrowArgumentException(nameof(s), "Invalid date info format.");

		return result;
	}

	/// <inheritdoc/>
	public static DateInfo Parse(string s, IFormatProvider? provider)
	{
		if (TryParse(s, provider, out DateInfo result) is false)
			ThrowHelper.ThrowArgumentException(nameof(s), "Invalid date info format.");

		return result;
	}

	/// <summary>Tries to parse the given <paramref name="s"/> into a <see cref="DateInfo"/> value.</summary>
	/// <param name="s">The <see langword="string"/> to parse.</param>
	/// <returns>The parsed <see cref="DateInfo"/> value, or <see langword="null"/> if parsing failed.</returns>
	public static DateInfo? TryParse([NotNullWhen(true)] string? s)
	{
		if (TryParse(s, null, out DateInfo result))
			return result;

		return null;
	}

	/// <summary>Tries to parse the given <paramref name="s"/> into a <see cref="DateInfo"/> value.</summary>
	/// <param name="s">The <see langword="string"/> to parse.</param>
	/// <param name="result">The parsed <see cref="DateInfo"/> value.</param>
	/// <returns><see langword="true"/> if the parsing was successful, <see langword="false"/> otherwise.</returns>
	public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out DateInfo result) => TryParse(s, null, out result);

	/// <inheritdoc/>
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out DateInfo result)
	{
		if (s is null)
		{
			result = default;
			return false;
		}

		if (s is "?")
		{
			result = default;
			return true;
		}

		int? year = null, month = null, day = null;
		string[] parts = s.Split('-');

		if (parts.Length >= 1 && int.TryParse(parts[0], out int y))
			year = y;

		if (parts.Length >= 2 && int.TryParse(parts[1], out int m))
			month = m;

		if (parts.Length >= 3 && int.TryParse(parts[2], out int d))
			day = d;

		if (year is null && month is null && day is null)
		{
			result = default;
			return false;
		}

		result = new(year, month, day);
		return true;
	}
	#endregion

	#region Helpers
	private string DebuggerDisplay()
	{
		List<string> parts = [];

		if (Year is not null)
			parts.Add($"{Year} = ({Year})");

		if (Month is not null)
			parts.Add($"{Month} = ({Month})");

		if (Day is not null)
			parts.Add($"{Day} = ({Day})");

		return $"{nameof(DateInfo)} {{ {string.Join(", ", parts)} }}";
	}
	#endregion
}
