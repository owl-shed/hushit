namespace OwlShed.Hushit.Common;

/// <summary>
/// 	Contains various extensions related to the <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanExtensions
{
	extension(TimeSpan time)
	{
		#region Methods
		/// <summary>Converts the time into a pretty formatted <see langword="string"/>.</summary>
		/// <param name="includeMilliseconds">Whether to include the milliseconds, this will be ignored if the total time is less than a second.</param>
		/// <returns>The pretty formatted time.</returns>
		public string ToPrettyTime(bool includeMilliseconds = true)
		{
			List<string> parts = [];

			if (time.Days > 0 || time.Hours > 0)
				parts.Add($"{(time.Days * 24) + time.Hours:n0}h");

			if (time.Minutes > 0)
				parts.Add($"{time.Minutes}m");

			if (time.Seconds > 0)
				parts.Add($"{time.Seconds}s");

			if (parts.Count is 0 || (includeMilliseconds && time.Milliseconds > 0))
				parts.Add($"{time.Milliseconds}ms");

			return string.Join(' ', parts);
		}
		#endregion
	}
}
