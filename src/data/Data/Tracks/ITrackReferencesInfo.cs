namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents a data model that contains references to tracks.
/// </summary>
public interface ITrackReferencesInfo : IDataModel
{
	#region Properties
	/// <summary>The ids of the related tracks.</summary>
	ReadOnlyObservableCollection<string> TrackIds { get; }
	#endregion
}
