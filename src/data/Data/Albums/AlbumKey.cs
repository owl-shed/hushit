namespace OwlShed.Hushit.Data.Albums;

internal sealed class AlbumKey : IEquatable<AlbumKey>
{
	#region Properties
	[JsonPropertyName("name")]
	public string Name { get; init; }

	[JsonPropertyName("artist_ids")]
	public string[] ArtistIds { get; }
	#endregion

	#region Constructors
	[JsonConstructor]
	public AlbumKey(string name, string[] artistIds) : this(name, (IReadOnlyCollection<string>)artistIds) { }
	public AlbumKey(string name, IReadOnlyCollection<string> artistIds)
	{
		Name = name;
		ArtistIds = artistIds.Order(StringComparer.Ordinal).ToArray();
	}
	#endregion

	#region Methods
	public bool Equals([NotNullWhen(true)] AlbumKey? other)
	{
		if (other is null)
			return false;

		if (Name != other.Name)
			return false;

		if (ArtistIds.Length != other.ArtistIds.Length)
			return false;

		for (int i = 0; i < ArtistIds.Length; i++)
		{
			if (ArtistIds[i] != other.ArtistIds[i])
				return false;
		}

		return true;
	}
	public override bool Equals(object? obj)
	{
		if (obj is AlbumKey other)
			return Equals(other);

		return false;
	}
	public override int GetHashCode()
	{
		HashCode code = new();
		code.Add(Name);
		code.Add(ArtistIds.Length);

		foreach (string artist in ArtistIds)
			code.Add(artist);

		return code.ToHashCode();
	}
	#endregion
}
