namespace OwlShed.Hushit.Data.Tracks.References;

/// <summary>
/// 	Represents a mutable version of the <see cref="ITrackReferencesInfo"/>.
/// </summary>
public interface IMutableTrackReferences : IMutableDataModel<IMutableTrackReferences, ITrackReferencesUpdate>
{
	#region Properties
	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	IList<string> TrackIds { get; set; }
	#endregion
}
