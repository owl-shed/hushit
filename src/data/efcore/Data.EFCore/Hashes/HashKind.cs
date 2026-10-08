namespace OwlShed.Hushit.Data.EFCore.Hashes;

/// <summary>
/// Represents the database model for hash kinds.
/// </summary>
[Table("hash_kinds")]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public sealed class HashKind
{
	#region Constants
	/// <summary>The special id for SHA256.</summary>
	public const string Sha256 = "sha256";
	#endregion

	#region Properties
	/// <summary>The id of the hash kind.</summary>
	[Column(Order = 0)]
	public string Id { get; init; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="HashKind"/>.</summary>
	/// <param name="id">The id of the hash kind.</param>
	public HashKind(string id) => Id = id;
	#endregion

	#region Methods
	[ExcludeFromCodeCoverage]
	private string DebuggerDisplay() => $"{nameof(HashKind)} {{ Id = ({Id}) }}";
	#endregion
}
