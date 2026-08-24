namespace OwlShed.Hushit.Data.Artists.References;

/// <summary>
/// 	Represents an update to the <see cref="IArtistReferencesInfo"/>.
/// </summary>
public interface IArtistReferencesUpdate
{
	#region Properties
	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	ListUpdate<string> ArtistIds { get; set; }
	#endregion
}
