namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents an update to the <see cref="IArtistInfo"/>.
/// </summary>
public interface IArtistUpdate : IAlbumReferencesUpdate<IArtistUpdate>, ITrackReferencesUpdate<IArtistUpdate>, IGenreReferencesUpdate<IArtistUpdate>
{
	#region Methods
	/// <summary>Sets the new name for the artist.</summary>
	/// <param name="name">The new name for the artist.</param>
	/// <returns>The used artist update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	IArtistUpdate WithName(string name);

	/// <summary>Adds a new alias for the artist.</summary>
	/// <param name="alias">The new alias.</param>
	/// <returns>The used artist update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given alias was empty.</exception>
	IArtistUpdate AddAlias(string alias);

	/// <summary>Removes an alias from the artist.</summary>
	/// <param name="alias">The alias to remove.</param>
	/// <returns>The used artist update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given alias was empty.</exception>
	IArtistUpdate RemoveAlias(string alias);

	/// <summary>Sets the new aliases for the artist.</summary>
	/// <param name="aliases">The new aliases to set for the artist.</param>
	/// <returns>The used artist update builder.</returns>
	/// <remarks>This will override all of the previous aliases.</remarks>
	/// <exception cref="ArgumentException">Thrown if any of the given aliases was empty.</exception>
	IArtistUpdate WithAliases(params IReadOnlyList<string> aliases);
	#endregion
}
