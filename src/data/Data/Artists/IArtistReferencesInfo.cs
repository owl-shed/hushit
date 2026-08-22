namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents a data model that contains references to artists.
/// </summary>
public interface IArtistReferencesInfo : IDataModel
{
	#region Properties
	/// <summary>The ids of the related artists.</summary>
	ReadOnlyObservableCollection<string> ArtistIds { get; }
	#endregion
}
