namespace OwlShed.Hushit.Data.Tracks.References;

/// <summary>
/// 	Represents an update to the <see cref="ITrackReferencesInfo"/>.
/// </summary>
public interface ITrackReferencesUpdate
{
	#region Properties
	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	ListUpdate<string> TrackIds { get; set; }
	#endregion
}
