namespace OwlShed.Hushit.Data.AudioFiles;

/// <summary>
/// 	Represents a mutable version of the <see cref="IAudioFileInfo"/>.
/// </summary>
public sealed class MutableAudioFile : MutableDataModelBase<MutableAudioFile, AudioFileUpdate>
{
	#region Properties
	/// <inheritdoc cref="IAudioFileInfo.Path"/>
	public string? Path { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.Hash"/>
	public HashInfo? Hash { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackName"/>
	public string? TrackName { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackId"/>
	public string? TrackId { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackDate"/>
	public DateInfo? TrackDate { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.AlbumName"/>
	public string? AlbumName { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.AlbumDate"/>
	public DateInfo? AlbumDate { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackArtists"/>
	public IList<string> TrackArtists { get; set; } = [];

	/// <inheritdoc cref="IAudioFileInfo.AlbumArtists"/>
	public IList<string> AlbumArtists { get; set; } = [];

	/// <inheritdoc cref="IAudioFileInfo.TrackGenres"/>
	public IList<string> TrackGenres { get; set; } = [];

	/// <inheritdoc cref="IAudioFileInfo.AlbumGenres"/>
	public IList<string> AlbumGenres { get; set; } = [];
	#endregion

	#region Builder methods
	/// <summary>Sets the new path for the audio file.</summary>
	/// <param name="path">The new path for the audio file.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <exception cref="ArgumentException">Thrown if the given path was empty.</exception>
	public MutableAudioFile WithPath(string path)
	{
		Guard.IsNotWhiteSpace(path);
		Path = path;

		return this;
	}

	/// <summary>Sets the new hash for the audio file.</summary>
	/// <param name="hash">The new hash for the audio file.</param>
	/// <returns>The used mutable audio file instance.</returns>
	public MutableAudioFile WithHash(HashInfo hash)
	{
		Hash = hash;
		return this;
	}

	/// <summary>Sets the new name for the track.</summary>
	/// <param name="name">The new name for the track.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	public MutableAudioFile WithTrackName(string name)
	{
		Guard.IsNotWhiteSpace(name);
		TrackName = name;

		return this;
	}

	/// <summary>Sets the new name for the album.</summary>
	/// <param name="name">The new name for the album.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <exception cref="ArgumentException">Thrown if the given name was empty.</exception>
	/// <remarks>A <see langword="null"/> value is allowed in order to remove the album name.</remarks>
	public MutableAudioFile WithAlbumName(string? name)
	{
		if (name is not null)
			Guard.IsNotNullOrWhiteSpace(name);

		AlbumName = name;
		return this;
	}

	/// <summary>Sets the new release date for the track.</summary>
	/// <param name="date">The date on which the track was released.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <remarks>A <see langword="null"/> value is allowed in order to remove the track date.</remarks>
	public MutableAudioFile WithTrackDate(DateInfo? date)
	{
		TrackDate = date;
		return this;
	}

	/// <summary>Sets the new release date for the album.</summary>
	/// <param name="date">The date on which the album was released.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <remarks>A <see langword="null"/> value is allowed in order to remove the album date.</remarks>
	public MutableAudioFile WithAlbumDate(DateInfo? date)
	{
		AlbumDate = date;
		return this;
	}

	/// <summary>Sets the new artists that made the track.</summary>
	/// <param name="artists">The artists that made the track.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <exception cref="ArgumentException">
	/// 	Thrown if any of the artist names were empty
	/// 	or only consisted of white-space characters.
	/// </exception>
	public MutableAudioFile WithTrackArtists(params IReadOnlyList<string> artists)
	{
		foreach (string artist in artists)
			Guard.IsNotWhiteSpace(artist, nameof(artists));

		TrackArtists = [.. artists];
		return this;
	}

	/// <summary>Sets the new artists that made the album.</summary>
	/// <param name="artists">The artists that made the album.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <exception cref="ArgumentException">
	/// 	Thrown if any of the artist names were empty
	/// 	or only consisted of white-space characters.
	/// </exception>
	public MutableAudioFile WithAlbumArtists(params IReadOnlyList<string> artists)
	{
		foreach (string artist in artists)
			Guard.IsNotWhiteSpace(artist, nameof(artists));

		AlbumArtists = [.. artists];
		return this;
	}

	/// <summary>Sets the new genres that the track belongs to.</summary>
	/// <param name="genres">The genres that the track belongs to.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <exception cref="ArgumentException">
	/// 	Thrown if any of the genre names were empty
	/// 	or only consisted of white-space characters.
	/// </exception>
	public MutableAudioFile WithTrackGenres(params IReadOnlyList<string> genres)
	{
		foreach (string genre in genres)
			Guard.IsNotWhiteSpace(genre, nameof(genres));

		TrackGenres = [.. genres];
		return this;
	}

	/// <summary>Sets the new genres that the album belongs to.</summary>
	/// <param name="genres">The genres that the album belongs to.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <exception cref="ArgumentException">
	/// 	Thrown if any of the genre names were empty
	/// 	or only consisted of white-space characters.
	/// </exception>
	public MutableAudioFile WithAlbumGenres(params IReadOnlyList<string> genres)
	{
		foreach (string genre in genres)
			Guard.IsNotWhiteSpace(genre, nameof(genres));

		AlbumGenres = [.. genres];
		return this;
	}

	/// <summary>Sets the id for the new track of the audio file.</summary>
	/// <param name="id">The id for the new track of the audio file. A <see langword="null"/> value can be used to remove audio file from the track.</param>
	/// <returns>The used mutable audio file instance.</returns>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="id"/> was empty.</exception>
	public MutableAudioFile WithTrack(string? id)
	{
		if (id is not null)
			Guard.IsNotWhiteSpace(id);

		TrackId = id;
		return this;
	}
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override AudioFileUpdate GetUpdateFrom(MutableAudioFile oldState)
	{
		if (Path is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Path)}' to have a value.");

		if (Hash is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Hash)}' to have a value.");

		if (TrackName is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(TrackName)}' to have a value.");

		return new()
		{
			Path = Update.Value(oldState.Path, Path),
			Hash = oldState.Hash is null ? Hash.Value : Update.Value(oldState.Hash, Hash),
			TrackName = Update.Value(oldState.TrackName, TrackName),
			TrackId = Update.Nullable(oldState.TrackId, TrackId),
			TrackDate = Update.Nullable(oldState.TrackDate, TrackDate),
			AlbumName = Update.Nullable(oldState.AlbumName, AlbumName),
			AlbumDate = Update.Nullable(oldState.AlbumDate, AlbumDate),
			TrackArtists = Update.List(oldState.TrackArtists, TrackArtists),
			AlbumArtists = Update.List(oldState.AlbumArtists, AlbumArtists),
			TrackGenres = Update.List(oldState.TrackGenres, TrackGenres),
			AlbumGenres = Update.List(oldState.AlbumGenres, AlbumGenres),
		};
	}
	#endregion
}

/// <summary>
/// 	Contains various extensions related to the <see cref="MutableAudioFile"/>.
/// </summary>
public static class IMutableAudioFileExtensions
{
	extension(MutableAudioFile update)
	{
		#region Methods
		/// <summary>Sets the new track of the audio file.</summary>
		/// <param name="track">The new track of the audio file. A <see langword="null"/> value can be used to remove audio file from the track.</param>
		/// <returns>The used mutable audio file instance.</returns>
		public MutableAudioFile WithTrack(ITrackInfo? track) => update.WithTrack(track?.Id);
		#endregion
	}
}
