namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents an update for the <see cref="IGenreInfo"/>.
/// </summary>
public interface IGenreUpdate : IArtistReferencesUpdate<IGenreUpdate>, IAlbumReferencesUpdate<IGenreUpdate>, ITrackReferencesUpdate<IGenreUpdate>
{
	#region Methods
	/// <summary>Sets the new name for the genre.</summary>
	/// <param name="name">The new name for the genre.</param>
	/// <returns>The used genre update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	IGenreUpdate WithName(string name);
	#endregion
}
