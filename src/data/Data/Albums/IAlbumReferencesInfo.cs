namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents a data model that contains references to albums.
/// </summary>
public interface IAlbumReferencesInfo : IDataModel
{
	#region Properties
	/// <summary>The ids of the related albums.</summary>
	ReadOnlyObservableCollection<string> AlbumIds { get; }
	#endregion
}
