namespace OwlShed.Hushit.Data.Images;

internal sealed class ImageInfo : DataModelBase<IImageInfo, MutableImage, ImageUpdate>, IImageInfo
{
	#region Fields
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly ObservableCollection<string> _audioFileIds = [];
	#endregion

	#region Properties
	/// <inheritdoc/>
	protected override IDataRepository<IImageInfo, MutableImage, ImageUpdate> Repository => Data.Images;

	/// <inheritdoc/>
	public string Path { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public HashInfo Hash { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public ReadOnlyObservableCollection<string> AudioFileIds => new(_audioFileIds);
	#endregion

	#region Constructors
	public ImageInfo(IHushitData data, string id, string path, HashInfo hash) : base(data, id)
	{
		Path = path;
		Hash = hash;
	}
	public ImageInfo(IHushitData data, string id, MutableImage initialState) : base(data, id)
	{
		CopyState(initialState);

		Debug.Assert(Path is not null);
	}
	#endregion

	#region Methods
	/// <inheritdoc/>
	public override MutableImage ToMutable()
	{
		return new()
		{
			Path = Path,
			Hash = Hash,
			AudioFileIds = [.. _audioFileIds],
		};
	}

	/// <inheritdoc/>
	internal override void CopyState(MutableImage state)
	{
		using ReaderWriterWriteLock _ = Lock.WriteLock();

		if (state.Path is null)
			ThrowHelper.ThrowArgumentException(nameof(state), $"Expected the new state to have a path.");

		if (state.Hash is null)
			ThrowHelper.ThrowArgumentException(nameof(state), $"Expected the new state to have a hash.");

		Path = state.Path;
		Hash = state.Hash.Value;
		_audioFileIds.Replace(state.AudioFileIds);
	}

	/// <inheritdoc/>
	public async IAsyncEnumerable<IAudioFileInfo> GetAudioFilesAsync([EnumeratorCancellation] CancellationToken cancellation = default)
	{
		foreach (string id in _audioFileIds)
		{
			IAudioFileInfo? file = await Data.AudioFiles.TryGetAsync(id, cancellation).ConfigureAwait(false);

			if (file is not null)
				yield return file;
		}
	}
	#endregion
}
