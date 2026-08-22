namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents an update to the <see cref="IArtistReferencesInfo"/>.
/// </summary>
public interface IArtistReferencesUpdate
{
	#region Methods
	/// <summary>Adds a new related artist.</summary>
	/// <param name="id">The id of the artist to add.</param>
	/// <returns>The used update builder.</returns>
	IArtistReferencesUpdate AddArtist(string id);

	/// <summary>Removes a related artist.</summary>
	/// <param name="id">The id of the artist to remove.</param>
	/// <returns>The used update builder.</returns>
	IArtistReferencesUpdate RemoveArtist(string id);

	/// <summary>Sets the new artist ids.</summary>
	/// <param name="artistIds">The ids of the new related artists.</param>
	/// <returns>The used update builder.</returns>
	IArtistReferencesUpdate WithArtists(params IReadOnlyList<string> artistIds);
	#endregion
}

/// <summary>
/// 	Represents an update to the <see cref="IArtistReferencesInfo"/>.
/// </summary>
/// <typeparam name="TBuilder">The type of the actual builder used.</typeparam>
public interface IArtistReferencesUpdate<TBuilder> : IArtistReferencesUpdate
	where TBuilder : notnull, IArtistReferencesUpdate<TBuilder>
{
	#region Methods
	/// <inheritdoc cref="IArtistReferencesUpdate.AddArtist(string)"/>
	new TBuilder AddArtist(string id);

	/// <inheritdoc cref="IArtistReferencesUpdate.RemoveArtist(string)"/>
	new TBuilder RemoveArtist(string id);

	/// <inheritdoc cref="IArtistReferencesUpdate.WithArtists(IReadOnlyList{string})"/>
	new TBuilder WithArtists(params IReadOnlyList<string> artistIds);

	IArtistReferencesUpdate IArtistReferencesUpdate.AddArtist(string id) => AddArtist(id);
	IArtistReferencesUpdate IArtistReferencesUpdate.RemoveArtist(string id) => RemoveArtist(id);
	IArtistReferencesUpdate IArtistReferencesUpdate.WithArtists(params IReadOnlyList<string> artistIds) => WithArtists(artistIds);
	#endregion
}
