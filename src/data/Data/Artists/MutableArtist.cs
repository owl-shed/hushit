namespace OwlShed.Hushit.Data.Artists;

/// <summary>
///   Represents a mutable version of the <see cref="IArtistInfo"/>.
/// </summary>
public sealed class MutableArtist :
	MutableDataModelBase<MutableArtist, ArtistUpdate>,
	IMutableAlbumReferences,
	IMutableTrackReferences,
	IMutableGenreReferences
{
	#region Properties
	/// <inheritdoc cref="IArtistInfo.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc cref="IArtistInfo.Aliases"/>
	public IList<string> Aliases { get; set; } = [];

	/// <inheritdoc/>
	public IList<string> GenreIds { get; set; } = [];

	/// <inheritdoc/>
	public IList<string> TrackIds { get; set; } = [];

	/// <inheritdoc/>
	public IList<string> AlbumIds { get; set; } = [];
	#endregion

	#region Builder methods
	/// <summary>Sets the new name for the artist.</summary>
	/// <param name="name">The new name for the artist.</param>
	/// <returns>The used artist update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	public MutableArtist WithName(string name)
	{
		Guard.IsNotWhiteSpace(name);

		Name = name;
		return this;
	}

	/// <summary>Adds a new alias for the artist.</summary>
	/// <param name="alias">The new alias.</param>
	/// <returns>The used artist update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given alias was empty.</exception>
	public MutableArtist AddAlias(string alias)
	{
		Guard.IsNotWhiteSpace(alias);


		Aliases.Add(alias);
		return this;
	}

	/// <summary>Removes an alias from the artist.</summary>
	/// <param name="alias">The alias to remove.</param>
	/// <returns>The used artist update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given alias was empty.</exception>
	public MutableArtist RemoveAlias(string alias)
	{
		Guard.IsNotWhiteSpace(alias);

		Aliases.Remove(alias);
		return this;
	}

	/// <summary>Sets the new aliases for the artist.</summary>
	/// <param name="aliases">The new aliases to set for the artist.</param>
	/// <returns>The used artist update builder.</returns>
	/// <remarks>This will override all of the previous aliases.</remarks>
	/// <exception cref="ArgumentException">Thrown if any of the given aliases was empty.</exception>
	public MutableArtist WithAliases(params IReadOnlyList<string> aliases)
	{
		foreach (string alias in aliases)
			Guard.IsNotWhiteSpace(alias, nameof(aliases));

		Aliases = [.. aliases];
		return this;
	}
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override ArtistUpdate GetUpdateFrom(MutableArtist oldState)
	{
		if (Name is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Name)}' to have a value.");

		return new()
		{
			Name = Update.Value(oldState.Name, Name),
			Aliases = Update.List(oldState.Aliases, Aliases),
			AlbumIds = Update.List(oldState.AlbumIds, AlbumIds),
			TrackIds = Update.List(oldState.TrackIds, TrackIds),
			GenreIds = Update.List(oldState.GenreIds, GenreIds),
		};
	}
	#endregion
}
