namespace OwlShed.Hushit.Data.AudioFiles;

internal sealed class AudioFileInfo : DataModelBase<IAudioFileInfo, MutableAudioFile, AudioFileUpdate>, IAudioFileInfo
{
	#region Fields
	// Todo(Nightowl): This needs to somehow ensure it's both observable, but also locked, and I'm not sure how to actually do that here;
	private readonly ObservableCollection<string> _trackArtists = [], _albumArtists = [], _trackGenres = [], _albumGenres = [];
	#endregion

	#region Properties
	/// <inheritdoc/>
	public string Path { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public HashInfo Hash { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public string TrackName { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public DateInfo? TrackDate { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public string? TrackId { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public string? AlbumName { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public DateInfo? AlbumDate { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public ReadOnlyObservableCollection<string> TrackArtists => field ??= new(_trackArtists);

	/// <inheritdoc/>
	public ReadOnlyObservableCollection<string> AlbumArtists => field ??= new(_albumArtists);

	/// <inheritdoc/>
	public ReadOnlyObservableCollection<string> TrackGenres => field ??= new(_trackGenres);

	/// <inheritdoc/>
	public ReadOnlyObservableCollection<string> AlbumGenres => field ??= new(_albumGenres);

	/// <inheritdoc/>
	protected override IDataRepository<IAudioFileInfo, MutableAudioFile, AudioFileUpdate> Repository => Data.AudioFiles;

	/// <inheritdoc/>
	public TimeSpan? Duration { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public int? TrackNumber { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public int? TotalTracks { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public string? ContainerFormat
	{
		get => Read(ref field);
		private set
		{
			if (TrySet(ref field, value))
				RaisePropertyChanged(nameof(DisplayFormat));
		}
	}

	/// <inheritdoc/>
	public string? AudioFormat
	{
		get => Read(ref field);
		private set
		{
			if (TrySet(ref field, value))
				RaisePropertyChanged(nameof(DisplayFormat));
		}
	}

	/// <inheritdoc/>
	public string? DisplayFormat
	{
		get
		{
			if (ContainerFormat is null)
				return AudioFormat;

			if (AudioFormat is null)
				return ContainerFormat;

			if (string.Equals(ContainerFormat, AudioFormat, StringComparison.OrdinalIgnoreCase))
				return ContainerFormat;

			return $"{ContainerFormat}/{AudioFormat}";
		}
	}

	/// <inheritdoc/>
	public int? BitRate { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public int? SampleRate { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public int? Channels { get => Read(ref field); private set => TrySet(ref field, value); }
	#endregion

	#region Constructors
	public AudioFileInfo(IHushitData data, string id, string path, HashInfo hash, string trackName) : base(data, id)
	{
		Path = path;
		Hash = hash;
		TrackName = trackName;
	}
	public AudioFileInfo(IHushitData data, string id, MutableAudioFile initialState) : base(data, id)
	{
		CopyState(initialState);

		Debug.Assert(Path is not null);
		Debug.Assert(TrackName is not null);
	}
	#endregion

	#region Methods
	/// <inheritdoc/>
	public async ValueTask<ITrackInfo?> GetTrackAsync(CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		string? id = TrackId;

		if (id is null)
			return null;

		return await Data.Tracks.TryGetAsync(id, cancellation).ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public async ValueTask<bool> ReloadAsync(CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		return await Data.AudioFiles.ReloadAsync(Id, cancellation);
	}

	/// <inheritdoc/>
	public async ValueTask<bool> TryReloadAsync(CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();
		return await Data.AudioFiles.TryReloadAsync(Id, cancellation);
	}
	public override MutableAudioFile ToMutable()
	{
		return new()
		{
			Path = Path,
			Hash = Hash,
			TrackName = TrackName,
			TrackId = TrackId,
			TrackDate = TrackDate,
			AlbumName = AlbumName,
			AlbumDate = AlbumDate,
			TrackArtists = [.. _trackArtists],
			AlbumArtists = [.. _albumArtists],
			TrackGenres = [.. _trackGenres],
			AlbumGenres = [.. _albumGenres],
			Duration = Duration,
			TrackNumber = TrackNumber,
			TotalTracks = TotalTracks,
			ContainerFormat = ContainerFormat,
			AudioFormat = AudioFormat,
			BitRate = BitRate,
			SampleRate = SampleRate,
			Channels = Channels,
		};
	}
	internal override void CopyState(MutableAudioFile state)
	{
		using ReaderWriterWriteLock _ = Lock.WriteLock();

		if (state.Path is null)
			ThrowHelper.ThrowArgumentException(nameof(state), $"Expected the new state to have a path.");

		if (state.Hash is null)
			ThrowHelper.ThrowArgumentException(nameof(state), $"Expected the new state to have a hash.");

		if (state.TrackName is null)
			ThrowHelper.ThrowArgumentException(nameof(state), $"Expected the new state to have a track name.");

		Path = state.Path;
		Hash = state.Hash.Value;
		TrackName = state.TrackName;
		TrackId = state.TrackId;
		TrackDate = state.TrackDate;
		AlbumName = state.AlbumName;
		AlbumDate = state.AlbumDate;

		_trackArtists.Replace(state.TrackArtists);
		_albumArtists.Replace(state.AlbumArtists);
		_trackGenres.Replace(state.TrackGenres);
		_albumGenres.Replace(state.AlbumGenres);

		Duration = state.Duration;
		TrackNumber = state.TrackNumber;
		TotalTracks = state.TotalTracks;
		ContainerFormat = state.ContainerFormat;
		AudioFormat = state.AudioFormat;
		BitRate = state.BitRate;
		SampleRate = state.SampleRate;
		Channels = state.Channels;
	}
	#endregion
}
