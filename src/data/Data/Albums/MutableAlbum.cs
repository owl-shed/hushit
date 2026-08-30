namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents a mutable version of the <see cref="IAlbumInfo"/>.
/// </summary>
public sealed class MutableAlbum :
	MutableDataModelBase<MutableAlbum, AlbumUpdate>,
	IMutableArtistReferences,
	IMutableTrackReferences,
	IMutableGenreReferences
{
	#region Properties
	/// <inheritdoc cref="IAlbumInfo.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc/>
	public IList<string> ArtistIds { get; set; } = [];

	/// <inheritdoc/>
	public IList<string> TrackIds { get; set; } = [];

	/// <inheritdoc/>
	public IList<string> GenreIds { get; set; } = [];
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override AlbumUpdate GetUpdateFrom(MutableAlbum oldState)
	{
		if (Name is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Name)}' to have a value.");

		return new()
		{
			Name = Update.Value(oldState.Name, Name),
			ArtistIds = Update.List(oldState.ArtistIds, ArtistIds),
			TrackIds = Update.List(oldState.TrackIds, TrackIds),
			GenreIds = Update.List(oldState.GenreIds, GenreIds),
		};
	}
	#endregion
}
