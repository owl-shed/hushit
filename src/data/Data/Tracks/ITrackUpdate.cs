namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents an update to the <see cref="ITrackInfo"/>.
/// </summary>
public interface ITrackUpdate
{
	#region Methods
	/// <summary>Sets the new name for the track.</summary>
	/// <param name="name">The new name for the track.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	ITrackUpdate WithName(string name);

	/// <summary>Sets the new duration for the track.</summary>
	/// <param name="duration">The new duration for the track.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// 	Thrown if the given <paramref name="duration"/> was less than <see cref="TimeSpan.Zero"/>.
	/// </exception>
	ITrackUpdate WithDuration(TimeSpan duration);
	#endregion
}
