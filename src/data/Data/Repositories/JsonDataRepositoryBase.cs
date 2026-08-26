namespace OwlShed.Hushit.Data.Repositories;

internal abstract class JsonDataRepositoryBase<TModel, TMutable, TUpdate, TTypedModel, TJson> : LocalDataRepositoryBase<TModel, TMutable, TUpdate, TTypedModel>
	where TModel : notnull, IDataModel<TMutable, TUpdate>
	where TMutable : notnull, IMutableDataModel<TMutable, TUpdate>, new()
	where TUpdate : notnull
	where TTypedModel : class, TModel
	where TJson : notnull
{
	#region Constants
	private const string FileName = "data.json";
	#endregion

	#region Properties
	protected abstract JsonTypeInfo<TJson> TypeInfo { get; }
	#endregion

	#region Constructors
	protected JsonDataRepositoryBase(IHushitData data, string baseDirectory) : base(data, baseDirectory) { }
	#endregion

	#region Persist methods
	protected override async ValueTask PersistAsync(TTypedModel model, string directory, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		TJson json = ToJson(model.ToMutable());

		string path = GetJsonPath(directory);
		using (FileStream file = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
			await JsonSerializer.SerializeAsync(file, json, TypeInfo, cancellation).ConfigureAwait(false);
	}
	protected override async ValueTask<TTypedModel?> TryLoadPersistedAsync(string id, string directory, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string path = GetJsonPath(directory);
		TJson? json;
		using (FileStream file = File.OpenRead(path))
			json = await JsonSerializer.DeserializeAsync(file, TypeInfo, cancellation).ConfigureAwait(false);

		// Todo(Nightowl): Log corrupted file;
		if (json is null)
			return null;

		TMutable mutable = FromJson(json);
		TTypedModel model = Create(id, mutable);

		return model;
	}
	protected override ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string path = GetJsonPath(directory);
		File.TryDelete(path);

		return default;
	}
	#endregion

	#region Methods
	protected abstract TJson ToJson(TMutable mutable);
	protected abstract TMutable FromJson(TJson json);
	#endregion

	#region Helpers
	private static string GetJsonPath(string directory) => Path.Combine(directory, FileName);
	#endregion
}
