namespace OwlShed.Hushit.Data.Albums.References;

/// <summary>
/// 	Represents a mutable version of the <see cref="IAlbumReferencesInfo"/>.
/// </summary>
public interface IMutableAlbumReferences
{
	#region Properties
	/// <inheritdoc cref="IAlbumReferencesInfo.AlbumIds"/>
	IList<string> AlbumIds { get; set; }
	#endregion
}
