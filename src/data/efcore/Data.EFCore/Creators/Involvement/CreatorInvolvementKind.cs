namespace OwlShed.Hushit.Data.EFCore.Creators.Involvement;

/// <summary>
/// Represents the database model for creator involvement kinds.
/// </summary>
[Table("creator_involvement_kinds")]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public sealed class CreatorInvolvementKind
{
	#region Constants
	/// <summary>The special id for artists.</summary>
	public const string ArtistId = "artist";

	/// <summary>The special id for composers.</summary>
	public const string ComposerId = "composer";

	/// <summary>The special id for lyricists.</summary>
	public const string LyricistId = "lyricist";
	#endregion

	#region Properties
	/// <summary>The id of the involvement kind.</summary>
	[Column(Order = 0)]
	public string Id { get; init; }

	/// <summary>Whether the involvement kind was created by the user.</summary>
	[Column(Order = 1)]
	public bool IsCustom { get; init; }

	/// <summary>The name of the involvement kind.</summary>
	[Column(Order = 2)]
	public string Name { get; init; }
	#endregion

	#region Constructors
	/// <summary>Creates a new custom <see cref="CreatorInvolvementKind"/>.</summary>
	/// <param name="id">The id of the involvement kind.</param>
	/// <param name="name">The name of the involvement kind.</param>
	/// <param name="isCustom">Whether the involvement kind was created by the user.</param>
	/// <remarks>
	/// If <paramref name="isCustom"/> is <see langword="true"/> then the <paramref name="id"/>
	/// should be <see cref="Guid.CreateVersion7()"/> formatted with <c>"N"</c>.
	/// </remarks>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="id"/> was not formatted properly.</exception>
	public CreatorInvolvementKind(string id, string name, bool isCustom = false)
	{
		Id = id;
		Name = name;
		IsCustom = isCustom;

		if (isCustom && (Guid.TryParseExact(id, "N", out _) is false))
			ThrowHelper.ThrowArgumentException(nameof(id), $"The given id ({id}) did not follow the expected formatting.");
	}
	#endregion

	#region Methods
	[ExcludeFromCodeCoverage]
	private string DebuggerDisplay() => $"{nameof(CreatorInvolvementKind)} {{ Id = ({Id}), Name = ({Name}) }}";
	#endregion
}
