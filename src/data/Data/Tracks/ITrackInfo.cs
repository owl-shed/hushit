namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents information about a track.
/// </summary>
public interface ITrackInfo : IDataModel<ITrackUpdate>
{
	#region Properties
	/// <summary>The name of the track.</summary>
	string Name { get; }

	/// <summary>The duration of the track.</summary>
	TimeSpan Duration { get; }
	#endregion
}
