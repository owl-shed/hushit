namespace OwlShed.Hushit.Data.Images;

internal sealed partial class ImageRepository : JsonDataRepositoryBase<IImageInfo, MutableImage, ImageUpdate, ImageInfo, ImageRepository.ImageJson>, IImageRepository
{
	#region Nested types
	[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
	[JsonSerializable(typeof(ImageJson))]
	private sealed partial class ImageJsonContext : JsonSerializerContext { }
	internal sealed class ImageJson
	{
		#region Properties
		[JsonPropertyName("path")]
		public required string Path { get; init; }

		[JsonPropertyName("hash")]
		public required string Hash { get; init; }
		#endregion
	}
	#endregion

	#region Constants
	private const string AudioFileBackRefName = "audio_files";
	private const string PreferredHash = "sha256";
	#endregion

	#region Properties
	protected override JsonTypeInfo<ImageJson> TypeInfo => ImageJsonContext.Default.ImageJson;
	private JsonDataIndex<string> HashIndex { get; }
	private ValueLock<string> HashLock { get; } = new();
	#endregion

	#region Constructors
	public ImageRepository(IHushitData data, string baseDirectory) : base(data, baseDirectory)
	{
		HashIndex = new(IndexDirectory, "by_hash");
	}
	#endregion

	#region Methods
	public override void Initialise()
	{
		base.Initialise();
		Data.AudioFiles.ModelUpdated.Subscribe(AudioFileUpdatedAsync);
	}
	#endregion

	#region React methods
	private async ValueTask AudioFileUpdatedAsync(ModelUpdateInfo<IAudioFileInfo, MutableAudioFile, AudioFileUpdate> update, CancellationToken cancellation)
	{
		if (update.Update.CoverImageId.HasChanged)
		{
			ImageInfo? oldImage = await TryGetCoreAsync(update.Old.CoverImageId, cancellation).ConfigureAwait(false);
			if (oldImage is not null)
			{
				oldImage.AudioFileIds.Remove(update.Model.Id);
				await SaveAudioFileBackReferencesAsync(oldImage, cancellation).ConfigureAwait(false);
			}

			ImageInfo? newImage = await TryGetCoreAsync(update.New.CoverImageId, cancellation).ConfigureAwait(false);
			if (newImage is not null)
			{
				newImage.AudioFileIds.Add(update.Model.Id);
				await SaveAudioFileBackReferencesAsync(newImage, cancellation).ConfigureAwait(false);
			}
		}
	}
	#endregion

	#region Backreference methods
	private async ValueTask SaveAudioFileBackReferencesAsync(IImageInfo image, CancellationToken cancellation = default)
	{
		await SaveBackreferencesAsync(image.Id, AudioFileBackRefName, image.AudioFileIds, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Persist methods
	public async ValueTask<IImageInfo> CreateAsync(string path, CancellationToken cancellation = default)
	{
		HashInfo hash = await GetHashAsync(path, PreferredHash, cancellation).ConfigureAwait(false);
		string hashStr = hash.ToString();

		await using IAsyncDisposable _ = await HashLock.LockAsync(hashStr, cancellation).ConfigureAwait(false);

		string id = CreateNewId();
		string hashId = await HashIndex.GetOrAddAsync(hashStr, id, cancellation).ConfigureAwait(false);

		IImageInfo? image;
		if (hashId != id)
		{
			image = await TryGetAsync(hashId, cancellation).ConfigureAwait(false);
			if (image is not null)
				return image;
		}

		image = await CreateAsync(id, async (mutable, cancellation) =>
		{
			string directory = GetDirectory(id);
			Directory.CreateDirectory(directory);

			string ext = Path.GetExtension(path);
			string destinationPath = Path.Combine(directory, "original" + ext);
			File.Copy(path, destinationPath);

			mutable.Path = Path.GetNormalised(destinationPath);
			mutable.Hash = hash;
		}).ConfigureAwait(false);

		return image;
	}
	protected override async ValueTask OnCreatedAsync(ImageInfo model, CancellationToken cancellation = default)
	{
		await base.OnCreatedAsync(model, cancellation).ConfigureAwait(false);
		await HashIndex.SetAsync(model.Id, model.Hash.ToString(), cancellation).ConfigureAwait(false);
	}
	protected override async ValueTask<ImageInfo?> TryLoadPersistedAsync(string id, string directory, CancellationToken cancellation = default)
	{
		ImageInfo? image = await base.TryLoadPersistedAsync(id, directory, cancellation).ConfigureAwait(false);

		if (image is null)
			return null;

		IReadOnlyList<string> audioFiles = await LoadBackreferencesAsync(id, AudioFileBackRefName, cancellation).ConfigureAwait(false);
		image.AudioFileIds.Replace(audioFiles);

		return image;
	}
	protected override async ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default)
	{
		await base.StopPersistingAsync(id, directory, cancellation).ConfigureAwait(false);

		await DeleteBackreferencesAsync(id, AudioFileBackRefName, cancellation).ConfigureAwait(false);
		await HashIndex.RemoveAsync(id, cancellation).ConfigureAwait(false);
	}
	#endregion

	#region Json methods
	protected override ImageJson ToJson(MutableImage mutable)
	{
		Debug.Assert(mutable.Path is not null);
		Debug.Assert(mutable.Hash is not null);

		return new()
		{
			Path = mutable.Path,
			Hash = mutable.Hash.Value.ToString(),
		};
	}
	protected override MutableImage FromJson(ImageJson json)
	{
		return new()
		{
			Path = json.Path,
			Hash = HashInfo.TryParse(json.Hash)
		};
	}
	#endregion

	#region Hash methods
	private static async ValueTask<HashInfo> GetHashAsync(string path, string kind, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string hash = kind switch
		{
			"sha256" => await Hash.Sha256FromFileAsync(path, cancellation).ConfigureAwait(false),

			_ => ThrowHelper.ThrowArgumentException<string>(nameof(kind), $"Unknown hash kind ({kind})."),
		};

		return new(kind, hash);
	}
	#endregion

	#region Helpers
	protected override ImageInfo Create(string id, MutableImage initialState) => new(Data, id, initialState);
	protected override void CopyState(ImageInfo model, MutableImage oldState, MutableImage newState, ImageUpdate update) => model.CopyState(newState);
	protected override void MarkAsDeleted(ImageInfo model) => model.Exists = false;
	#endregion
}
