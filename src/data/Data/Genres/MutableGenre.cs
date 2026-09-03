namespace OwlShed.Hushit.Data.Genres;

/// <summary>
///   Represents a mutable version of the <see cref="IGenreInfo"/>.
/// </summary>
public sealed class MutableGenre : MutableDataModelBase<MutableGenre, GenreUpdate>
{
	#region Properties
	/// <inheritdoc cref="IGenreInfo.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	public IList<string> ArtistIds { get; set; } = [];

	/// <inheritdoc cref="IAlbumReferencesInfo.AlbumIds"/>
	public IList<string> AlbumIds { get; set; } = [];

	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	public IList<string> TrackIds { get; set; } = [];
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
