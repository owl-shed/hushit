namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents an update to the <see cref="IAlbumInfo"/>.
/// </summary>
public interface IAlbumUpdate : IArtistReferencesUpdate<IAlbumUpdate>, ITrackReferencesUpdate<IAlbumUpdate>, IGenreReferencesUpdate<IAlbumUpdate>
{
	#region Methods
	/// <summary>Sets the new name for the album.</summary>
	/// <param name="name">The new name for the album.</param>
	/// <returns>The used album update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	IAlbumUpdate WithName(string name);
	#endregion
}
