namespace OwlShed.Hushit.Data.Artists;

/// <summary>
///   Represents a mutable version of the <see cref="IArtistInfo"/>.
/// </summary>
public sealed class MutableArtist : MutableDataModelBase<MutableArtist, ArtistUpdate>
{
	#region Properties
	/// <inheritdoc cref="IArtistInfo.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc cref="IArtistInfo.Aliases"/>
	public IList<string> Aliases { get; set; } = [];

	/// <inheritdoc cref="IGenreReferencesInfo.GenreIds"/>
	public IList<string> GenreIds { get; set; } = [];

	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	public IList<string> TrackIds { get; set; } = [];

	/// <inheritdoc cref="IAlbumReferencesInfo.AlbumIds"/>
	public IList<string> AlbumIds { get; set; } = [];
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
