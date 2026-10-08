namespace OwlShed.Hushit.Data.Indices;

/// <summary>
/// 	Represents a data index that uses a JSON file for storage.
/// </summary>
/// <typeparam name="TValue">The type of the lookup value in the index.</typeparam>
public sealed partial class JsonDataIndex<TValue> : IDataIndex<TValue>
	where TValue : notnull
{
	#region Nested types
	[JsonSourceGenerationOptions(WriteIndented = true)]
	[JsonSerializable(typeof(Dictionary<string, string>))]
	private sealed partial class Context : JsonSerializerContext
	{
	}
	#endregion

	#region Fields
	private readonly SemaphoreSlim _lock = new(1, 1);
	private Dictionary<string, TValue>? _forward;
	private Dictionary<TValue, List<string>>? _backward;
	#endregion

	#region Properties
	/// <summary>The path of the JSON index file.</summary>
	public string Path { get; }
	private JsonTypeInfo<Dictionary<string, TValue>>? FallbackTypeInfo { get; set; }
	private JsonTypeInfo<Dictionary<string, TValue>> TypeInfo { get; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="JsonDataIndex{TValue}"/> instance.</summary>
	/// <param name="path">The path of the JSON index file.</param>
	public JsonDataIndex(string path)
	{
		Path = path;
		TypeInfo = GetTypeInfo();
	}

	/// <summary>Creates a new <see cref="JsonDataIndex{TValue}"/> instance.</summary>
	/// <param name="path">The path of the JSON index file.</param>
	/// <param name="typeInfo">The type info for serialising the json data.</param>
	public JsonDataIndex(string path, JsonTypeInfo<Dictionary<string, TValue>> typeInfo)
	{
		Path = path;
		FallbackTypeInfo = typeInfo;
		TypeInfo = GetTypeInfo();
	}

	/// <summary>Creates a new <see cref="JsonDataIndex{TValue}"/> instance.</summary>
	/// <param name="directory">The directory to store the index file in.</param>
	/// <param name="name">The name (without the extension) of the file to store the index data in.</param>
	public JsonDataIndex(string directory, string name)
	{
		Path = System.IO.Path.Combine(directory, $"{name}.json");
		TypeInfo = GetTypeInfo();
	}

	/// <summary>Creates a new <see cref="JsonDataIndex{TValue}"/> instance.</summary>
	/// <param name="directory">The directory to store the index file in.</param>
	/// <param name="name">The name (without the extension) of the file to store the index data in.</param>
	/// <param name="typeInfo">The type info for serialising the json data.</param>
	public JsonDataIndex(string directory, string name, JsonTypeInfo<Dictionary<string, TValue>> typeInfo)
	{
		Path = System.IO.Path.Combine(directory, $"{name}.json");
		FallbackTypeInfo = typeInfo;
		TypeInfo = GetTypeInfo();
	}
	#endregion

	#region Methods
	/// <inheritdoc/>
	public async ValueTask SetAsync(string id, TValue key, CancellationToken cancellation = default)
	{
		using (await _lock.LockAsync(cancellation).ConfigureAwait(false))
		{
			await EnsureLoadedAsync(cancellation).ConfigureAwait(false);
			Debug.Assert(_forward is not null);
			Debug.Assert(_backward is not null);

			_forward[id] = key;
			RecalculateBackward();

			await SaveAsync(cancellation).ConfigureAwait(false);
		}
	}

	/// <inheritdoc/>
	public async ValueTask RemoveAsync(string id, CancellationToken cancellation = default)
	{
		using (await _lock.LockAsync(cancellation).ConfigureAwait(false))
		{
			await EnsureLoadedAsync(cancellation).ConfigureAwait(false);
			Debug.Assert(_forward is not null);

			_forward.Remove(id);
			RecalculateBackward();

			await SaveAsync(cancellation).ConfigureAwait(false);
		}
	}

	/// <inheritdoc/>
	public async ValueTask<IReadOnlyList<string>> GetAsync(TValue key, CancellationToken cancellation = default)
	{
		using (await _lock.LockAsync(cancellation).ConfigureAwait(false))
		{
			await EnsureLoadedAsync(cancellation).ConfigureAwait(false);
			Debug.Assert(_backward is not null);

			if (_backward.TryGetValue(key, out List<string>? ids) && ids.Any())
				return ids;

			return [];
		}
	}

	/// <inheritdoc/>
	public async ValueTask<string> GetOrAddAsync(TValue key, string id, CancellationToken cancellation = default)
	{
		using (await _lock.LockAsync(cancellation).ConfigureAwait(false))
		{
			await EnsureLoadedAsync(cancellation).ConfigureAwait(false);
			Debug.Assert(_forward is not null);
			Debug.Assert(_backward is not null);

			if (_backward.TryGetValue(key, out List<string>? ids) && ids.Any())
				return ids[0];

			_forward[id] = key;
			RecalculateBackward();

			await SaveAsync(cancellation).ConfigureAwait(false);
			return id;
		}
	}

	/// <inheritdoc/>
	public async IAsyncEnumerable<IndexPair<TValue>> GetAllAsync([EnumeratorCancellation] CancellationToken cancellation = default)
	{
		using (await _lock.LockAsync(cancellation).ConfigureAwait(false))
		{
			await EnsureLoadedAsync(cancellation).ConfigureAwait(false);
			Debug.Assert(_forward is not null);

			foreach (KeyValuePair<string, TValue> pair in _forward)
				yield return new(pair.Key, pair.Value);
		}
	}
	#endregion

	#region Helpers
	private async ValueTask EnsureLoadedAsync(CancellationToken cancellation = default)
	{
		if (_forward is not null)
			return;

		if (File.Exists(Path) is false)
		{
			_forward = [];
			_backward = [];

			return;
		}

		using (FileStream file = File.OpenRead(Path))
		{
			_forward = await JsonSerializer.DeserializeAsync(file, TypeInfo, cancellation).ConfigureAwait(false);

			if (_forward is null)
				ThrowHelper.ThrowInvalidDataException($"Failed to deserialize the index file '{Path}'.");

			RecalculateBackward();
		}
	}
	private async ValueTask SaveAsync(CancellationToken cancellation = default)
	{
		string? directory = System.IO.Path.GetDirectoryName(Path);
		Debug.Assert(directory is not null);

		Directory.CreateDirectory(directory);

		using (FileStream file = File.Create(Path))
		{
			Debug.Assert(_forward is not null);
			await JsonSerializer.SerializeAsync(file, _forward, TypeInfo, cancellation).ConfigureAwait(false);
		}
	}
	private JsonTypeInfo<Dictionary<string, TValue>> GetTypeInfo()
	{
		JsonTypeInfo<Dictionary<string, TValue>>? typeInfo = null;
		typeInfo ??= (JsonTypeInfo<Dictionary<string, TValue>>?)Context.Default.GetTypeInfo(typeof(Dictionary<string, TValue>));
		typeInfo ??= FallbackTypeInfo;

		if (typeInfo is null)
			ThrowHelper.ThrowNotSupportedException($"The type ({typeof(TValue)}) is not supported, specify it through the {nameof(FallbackTypeInfo)}.");

		return typeInfo;
	}
	private void RecalculateBackward()
	{
		Debug.Assert(_forward is not null);

		_backward = [];
		foreach (KeyValuePair<string, TValue> pair in _forward)
		{
			if (_backward.TryGetValue(pair.Value, out List<string>? list) is false)
			{
				list = [];
				_backward.Add(pair.Value, list);
			}

			list.Add(pair.Key);
		}
	}
	#endregion
}
