namespace OwlShed.Hushit.Data.Albums.References;

/// <summary>
/// 	Represents an update to the <see cref="IAlbumReferencesInfo"/>.
/// </summary>
public interface IAlbumReferencesUpdate
{
	#region Properties
	/// <inheritdoc cref="IAlbumReferencesInfo.AlbumIds"/>
	ListUpdate<string> AlbumIds { get; set; }
	#endregion
}
