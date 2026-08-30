namespace OwlShed.Hushit.Data.Artists.References;

/// <summary>
/// 	Represents a mutable version of the <see cref="IArtistReferencesInfo"/>.
/// </summary>
public interface IMutableArtistReferences
{
	#region Properties
	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	IList<string> ArtistIds { get; set; }
	#endregion
}
