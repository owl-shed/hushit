namespace OwlShed.Hushit.Data.Tracks;

internal sealed class TrackInfo : DataModelBase<ITrackInfo, MutableTrack, TrackUpdate>, ITrackInfo
{
	#region Properties
	/// <inheritdoc/>
	protected override IDataRepository<ITrackInfo, MutableTrack, TrackUpdate> Repository => Data.Tracks;

	/// <inheritdoc/>
	public string Name { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public TimeSpan Duration { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public string? AlbumId { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public string? AudioFileId { get => Read(ref field); internal set => TrySet(ref field, value); }

	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> ArtistIds { get; } = [];

	/// <inheritdoc cref="IGenreReferencesInfo.GenreIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> GenreIds { get; } = [];

	ReadOnlyObservableCollection<string> IArtistReferencesInfo.ArtistIds => new(ArtistIds);
	ReadOnlyObservableCollection<string> IGenreReferencesInfo.GenreIds => new(GenreIds);
	#endregion

	#region Constructors
	public TrackInfo(IHushitData data, string id, string name, TimeSpan duration) : base(data, id)
	{
		Name = name;
		Duration = duration;
	}
	public TrackInfo(IHushitData data, string id, MutableTrack initialState) : base(data, id)
	{
		CopyState(initialState);

		Debug.Assert(Name is not null);
	}
	#endregion


	#region Methods
	/// <inheritdoc/>
	public async ValueTask<IAudioFileInfo?> GetAudioFileAsync(CancellationToken cancellation = default)
	{
		if (AudioFileId is null)
			return null;

		return await Data.AudioFiles.TryGetAsync(AudioFileId, cancellation).ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public async ValueTask<IAlbumInfo?> GetAlbumAsync(CancellationToken cancellation = default)
	{
		if (AlbumId is null)
			return null;

		return await Data.Albums.TryGetAsync(AlbumId, cancellation).ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public IAsyncEnumerable<IArtistInfo> GetArtistsAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Artists, ArtistIds, cancellation);

	/// <inheritdoc/>
	public IAsyncEnumerable<IGenreInfo> GetGenresAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Genres, GenreIds, cancellation);

	/// <inheritdoc/>
	public override MutableTrack ToMutable()
	{
		return new()
		{
			Name = Name,
			Duration = Duration,
			AlbumId = AlbumId,
			ArtistIds = ArtistIds.ToArray(),
			GenreIds = GenreIds.ToArray(),
		};
	}

	/// <inheritdoc/>
	internal override void CopyState(MutableTrack state)
	{
		using ReaderWriterWriteLock _ = Lock.WriteLock();

		if (state.Name is null)
			ThrowHelper.ThrowArgumentException(nameof(state), $"Expected the new state to have a name.");

		Name = state.Name;
		Duration = state.Duration;
		AlbumId = state.AlbumId;
		ArtistIds.Replace(state.ArtistIds);
		GenreIds.Replace(state.GenreIds);
	}
	#endregion
}
