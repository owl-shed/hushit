namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents a mutable version of the <see cref="ITrackInfo"/>.
/// </summary>
public sealed class MutableTrack : MutableDataModelBase<MutableTrack, TrackUpdate>
{
	#region Properties
	/// <inheritdoc cref="ITrackInfo.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc cref="ITrackInfo.Duration"/>
	public TimeSpan Duration { get; set; }

	/// <inheritdoc cref="ITrackInfo.AlbumId"/>
	public string? AlbumId { get; set; }

	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	public IList<string> ArtistIds { get; set; } = [];

	/// <inheritdoc cref="IGenreReferencesInfo.GenreIds"/>
	public IList<string> GenreIds { get; set; } = [];
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override TrackUpdate GetUpdateFrom(MutableTrack oldState)
	{
		if (Name is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Name)}' to have a value.");

		return new()
		{
			Name = Update.Value(oldState.Name, Name),
			Duration = Update.Value(oldState.Duration, Duration),
			AlbumId = Update.Nullable(oldState.AlbumId, AlbumId),
			ArtistIds = Update.List(oldState.ArtistIds, ArtistIds),
			GenreIds = Update.List(oldState.GenreIds, GenreIds),
		};
	}
	#endregion
}
