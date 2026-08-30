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
	private const string PreferredHash = "sha256";
	#endregion

	#region Properties
	protected override JsonTypeInfo<ImageJson> TypeInfo => ImageJsonContext.Default.ImageJson;
	private JsonDataIndex<string> HashIndex { get; }
	#endregion

	#region Constructors
	public ImageRepository(IHushitData data, string baseDirectory) : base(data, baseDirectory)
	{
		HashIndex = new(IndexDirectory, "by_hash");
	}
	#endregion

	#region Persist methods
	public async ValueTask<IImageInfo> CreateAsync(string path, CancellationToken cancellation = default)
	{
		string id = CreateNewId();
		HashInfo hash = await GetHashAsync(path, PreferredHash, cancellation).ConfigureAwait(false);
		string hashId = await HashIndex.GetOrAddAsync(hash.ToString(), id, cancellation).ConfigureAwait(false);

		IImageInfo? image = null;
		if (hashId != id)
		{
			image = await TryGetAsync(hashId, cancellation).ConfigureAwait(false);
			if (image is not null)
				return image;

			await HashIndex.RemoveAsync(hashId, cancellation).ConfigureAwait(false);
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
	protected override async ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default)
	{
		await base.StopPersistingAsync(id, directory, cancellation).ConfigureAwait(false);
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
