namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents a mutable version of the <see cref="ITrackInfo"/>.
/// </summary>
public sealed class MutableTrack :
	MutableDataModelBase<MutableTrack, TrackUpdate>,
	IMutableArtistReferences,
	IMutableGenreReferences
{
	#region Properties
	/// <inheritdoc cref="ITrackInfo.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc cref="ITrackInfo.Duration"/>
	public TimeSpan? Duration { get; set; }

	/// <inheritdoc cref="ITrackInfo.AlbumId"/>
	public string? AlbumId { get; set; }

	/// <inheritdoc cref="ITrackInfo.AudioFileId"/>
	public string? AudioFileId { get; set; }

	/// <inheritdoc/>
	public IList<string> ArtistIds { get; set; } = [];

	/// <inheritdoc/>
	public IList<string> GenreIds { get; set; } = [];
	#endregion

	#region Builder methods
	/// <summary>Sets the new name for the track.</summary>
	/// <param name="name">The new name for the track.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="name"/> was empty.</exception>
	public MutableTrack WithName(string name)
	{
		Guard.IsNotWhiteSpace(name);

		Name = name;
		return this;
	}

	/// <summary>Sets the new duration for the track.</summary>
	/// <param name="duration">The new duration for the track.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// 	Thrown if the given <paramref name="duration"/> was less than <see cref="TimeSpan.Zero"/>.
	/// </exception>
	public MutableTrack WithDuration(TimeSpan duration)
	{
		Guard.IsGreaterThanOrEqualTo(duration, TimeSpan.Zero);

		Duration = duration;
		return this;
	}

	/// <summary>Sets the id for the new album of the track.</summary>
	/// <param name="id">The id for the new album of the track. A <see langword="null"/> value can be used to remove track from the album.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="id"/> was empty.</exception>
	public MutableTrack WithAlbum(string? id)
	{
		if (id is not null)
			Guard.IsNotWhiteSpace(id);

		AlbumId = id;
		return this;
	}

	/// <summary>Sets the id for the new audio file of the track.</summary>
	/// <param name="id">The id for the new audio file of the track. A <see langword="null"/> value can be used to remove track from the audio file.</param>
	/// <returns>The used track update builder.</returns>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="id"/> was empty.</exception>
	public MutableTrack WithAudioFile(string? id)
	{
		if (id is not null)
			Guard.IsNotWhiteSpace(id);

		AudioFileId = id;
		return this;
	}
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override TrackUpdate GetUpdateFrom(MutableTrack oldState)
	{
		if (Name is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Name)}' to have a value.");

		if (Duration is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Duration)}' to have a value.");

		return new()
		{
			Name = Update.Value(oldState.Name, Name),
			Duration = oldState.Duration is null ? Duration.Value : Update.Value(oldState.Duration, Duration),
			AlbumId = Update.Nullable(oldState.AlbumId, AlbumId),
			AudioFileId = Update.Nullable(oldState.AudioFileId, AudioFileId),
			ArtistIds = Update.List(oldState.ArtistIds, ArtistIds),
			GenreIds = Update.List(oldState.GenreIds, GenreIds),
		};
	}
	#endregion
}

/// <summary>
/// 	Contains various extensions related to the <see cref="MutableTrack"/>.
/// </summary>
public static class MutableTrackExtensions
{
	extension(MutableTrack track)
	{
		#region Methods
		/// <summary>Sets the new album of the track.</summary>
		/// <param name="album">The new album of the track. A <see langword="null"/> value can be used to remove track from the album.</param>
		/// <returns>The used track update builder.</returns>
		public MutableTrack WithAlbum(IAlbumInfo? album) => track.WithAlbum(album?.Id);

		/// <summary>Sets the new audio file of the track.</summary>
		/// <param name="file">The new audio file of the track. A <see langword="null"/> value can be used to remove track from the audio file.</param>
		/// <returns>The used track update builder.</returns>
		public MutableTrack WithAudioFile(IAudioFileInfo? file) => track.WithAudioFile(file?.Id);
		#endregion
	}
}
