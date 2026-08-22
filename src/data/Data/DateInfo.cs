namespace OwlShed.Hushit.Data;

/// <summary>
/// 	Represents potentially partial information about a date.
/// </summary>
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public readonly struct DateInfo
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
