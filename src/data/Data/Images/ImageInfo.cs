namespace OwlShed.Hushit.Data.Images;

internal sealed class ImageInfo : DataModelBase<IImageInfo, MutableImage, ImageUpdate>, IImageInfo
{
	#region Properties
	/// <inheritdoc/>
	protected override IDataRepository<IImageInfo, MutableImage, ImageUpdate> Repository => Data.Images;

	/// <inheritdoc/>
	public string Path { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public HashInfo Hash { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc cref="IAudioFileReferencesInfo.AudioFileIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> AudioFileIds { get; } = [];
	ReadOnlyObservableCollection<string> IAudioFileReferencesInfo.AudioFileIds => new(AudioFileIds);
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
	}

	/// <inheritdoc/>
	public IAsyncEnumerable<IAudioFileInfo> GetAudioFilesAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.AudioFiles, AudioFileIds, cancellation);
	#endregion
}
