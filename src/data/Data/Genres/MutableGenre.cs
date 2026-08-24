namespace OwlShed.Hushit.Data.Genres;

/// <summary>
///   Represents a mutable version of the <see cref="IGenreInfo"/>.
/// </summary>
public sealed class MutableGenre :
	MutableDataModelBase<MutableGenre, GenreUpdate>,
	IMutableArtistReferences,
	IMutableAlbumReferences,
	IMutableTrackReferences
{
	#region Properties
	/// <inheritdoc cref="IGenreInfo.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc/>
	public IList<string> ArtistIds { get; set; } = [];

	/// <inheritdoc/>
	public IList<string> AlbumIds { get; set; } = [];

	/// <inheritdoc/>
	public IList<string> TrackIds { get; set; } = [];
	#endregion

	#region Builder methods
	/// <summary>Sets the new name for the genre.</summary>
	/// <param name="name">The new name for the genre.</param>
	/// <returns>The used genre update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	public MutableGenre WithName(string name)
	{
		Guard.IsNotWhiteSpace(name);
		Name = name;

		return this;
	}
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override GenreUpdate GetUpdateFrom(MutableGenre oldState)
	{
		if (Name is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Name)}' to have a value.");

		return new()
		{
			Name = Update.Value(oldState.Name, Name),
			ArtistIds = Update.List(oldState.ArtistIds, ArtistIds),
			AlbumIds = Update.List(oldState.AlbumIds, AlbumIds),
			TrackIds = Update.List(oldState.TrackIds, TrackIds),
		};
	}
	#endregion
}
