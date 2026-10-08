namespace OwlShed.Hushit.Data.Repositories;

internal abstract class LocalDataRepositoryBase<TModel, TMutable, TUpdate, TTypedModel> : DataRepositoryBase<TModel, TMutable, TUpdate, TTypedModel>
	where TModel : notnull, IDataModel<TMutable, TUpdate>
	where TMutable : notnull, IMutableDataModel<TMutable, TUpdate>, new()
	where TUpdate : notnull
	where TTypedModel : class, TModel
{
	#region Properties
	protected string BaseDirectory { get; }
	#endregion

	#region Constructors
	protected LocalDataRepositoryBase(IHushitData data, string baseDirectory) : base(data)
	{
		BaseDirectory = baseDirectory;
	}
	#endregion

	#region Persist methods
	protected sealed override async ValueTask PersistAsync(TTypedModel model, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string directory = GetDirectory(model.Id);
		Directory.CreateDirectory(directory);

		await PersistAsync(model, directory, cancellation).ConfigureAwait(false);
	}
	protected abstract ValueTask PersistAsync(TTypedModel model, string directory, CancellationToken cancellation = default);
	protected sealed override async IAsyncEnumerable<string> GetPersistedIdsAsync([EnumeratorCancellation] CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string[] level1 = Directory.GetDirectories(BaseDirectory);
		string[] level2 = level1.SelectMany(Directory.GetDirectories).ToArray();
		string[] idDirectories = level2.SelectMany(Directory.GetDirectories).ToArray();

		foreach (string directory in idDirectories)
		{
			cancellation.ThrowIfCancellationRequested();

			string? id = Path.GetFileName(directory);
			Debug.Assert(id is not null);

			yield return id;
		}
	}
	protected sealed override async ValueTask<TTypedModel?> TryLoadPersistedAsync(string id, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string directory = GetDirectory(id);
		return await TryLoadPersistedAsync(id, directory, cancellation).ConfigureAwait(false);
	}
	protected abstract ValueTask<TTypedModel?> TryLoadPersistedAsync(string id, string directory, CancellationToken cancellation = default);
	protected sealed override async ValueTask StopPersistingAsync(string id, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string directory = GetDirectory(id);
		await StopPersistingAsync(id, directory, cancellation).ConfigureAwait(false);

		Directory.DeleteHierarchyIfEmpty(directory);

		string? level2 = Path.GetDirectoryName(directory);
		string? level1 = Path.GetDirectoryName(level2);

		Directory.DeleteIfEmpty(level2);
		Directory.DeleteIfEmpty(level1);
		Directory.DeleteIfEmpty(BaseDirectory);
	}
	protected abstract ValueTask StopPersistingAsync(string id, string directory, CancellationToken cancellation = default);
	#endregion

	#region Back reference methods
	protected virtual async ValueTask SaveBackreferencesAsync(string id, string kind, IReadOnlyList<string> references, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string directory = GetBackreferenceDirectory(id);
		string path = Path.Combine(directory, kind);

		Directory.CreateDirectory(directory);
		await File.WriteAllLinesAsync(path, references, cancellation).ConfigureAwait(false);
	}
	protected virtual async ValueTask SaveBackreferenceAsync(string id, string kind, string? reference, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		if (reference is null)
		{
			await DeleteBackreferencesAsync(id, kind, cancellation).ConfigureAwait(false);
			return;
		}

		string directory = GetBackreferenceDirectory(id);
		string path = Path.Combine(directory, kind);

		Directory.CreateDirectory(directory);
		await File.WriteAllTextAsync(path, reference, cancellation).ConfigureAwait(false);
	}
	protected virtual async ValueTask<IReadOnlyList<string>> LoadBackreferencesAsync(string id, string kind, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string directory = GetBackreferenceDirectory(id);
		string path = Path.Combine(directory, kind);

		if (File.Exists(path) is false)
			return [];

		string[] references = await File.ReadAllLinesAsync(path, cancellation).ConfigureAwait(false);

		return references
			.Select(static r => r.Trim())
			.Where(r => string.IsNullOrWhiteSpace(r) is false)
			.ToArray();
	}
	protected virtual async ValueTask<string?> LoadBackreferenceAsync(string id, string kind, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string directory = GetBackreferenceDirectory(id);
		string path = Path.Combine(directory, kind);

		if (File.Exists(path) is false)
			return null;

		string reference = await File.ReadAllTextAsync(path, cancellation).ConfigureAwait(false);
		reference = reference.Trim();

		if (reference.IsWhiteSpace())
			return null;

		return reference;
	}
	protected virtual async ValueTask DeleteBackreferencesAsync(string id, string kind, CancellationToken cancellation = default)
	{
		cancellation.ThrowIfCancellationRequested();

		string directory = GetBackreferenceDirectory(id);
		string path = Path.Combine(directory, kind);

		File.TryDelete(path);
		Directory.DeleteIfEmpty(directory);
	}
	#endregion

	#region Directory methods
	protected string GetDirectory(string id)
	{
		string level1 = Path.Combine(BaseDirectory, $"{id[0]}");
		string level2 = Path.Combine(level1, $"{id[1]}");
		string idDirectory = Path.Combine(level2, id);

		return idDirectory;
	}
	protected string GetBackreferenceDirectory(string id)
	{
		string directory = GetDirectory(id);
		string backReferences = Path.Combine(directory, "backrefs");

		return backReferences;
	}
	#endregion
}
