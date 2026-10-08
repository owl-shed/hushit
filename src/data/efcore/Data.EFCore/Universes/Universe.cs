namespace OwlShed.Hushit.Data.EFCore.Universes;

/// <summary>
/// Represents the database model for a universe.
/// </summary>
[Table("universes")]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public sealed class Universe
{
	#region Constructors
	/// <summary>The special id for the real universe.</summary>
	/// <remarks>Let's not debate what real means.</remarks>
	public const string RealId = "real";
	#endregion

	#region Properties
	/// <summary>The id of the universe.</summary>
	[Column(Order = 0)]
	public string Id { get; init; }

	/// <summary>Whether the universe was created by the user.</summary>
	[Column(Order = 1)]
	public bool IsCustom { get; init; }

	/// <summary>The name of the universe.</summary>
	[Column(Order = 3)]
	public string Name { get; init; }

	/// <summary>The id of the parent universe.</summary>
	[Column(Order = 2)]
	public string? ParentId { get; init; }
	#endregion

	#region Constructors
	/// <summary>Creates a new custom <see cref="Universe"/>.</summary>
	/// <param name="id">The id of the universe.</param>
	/// <param name="parentId">The id of the parent universe.</param>
	/// <param name="name">The name of the universe.</param>
	/// <param name="isCustom">Whether the involvement kind was created by the user.</param>
	/// <remarks>
	/// If <paramref name="isCustom"/> is <see langword="true"/> then the <paramref name="id"/>
	/// should be <see cref="Guid.CreateVersion7()"/> formatted with <c>"N"</c>.
	/// </remarks>
	/// <exception cref="ArgumentException">Thrown if the given <paramref name="id"/> was not formatted properly.</exception>
	public Universe(string id, string? parentId, string name, bool isCustom = false)
	{
		Id = id;
		ParentId = parentId;
		Name = name;
		IsCustom = isCustom;

		if (isCustom && (Guid.TryParseExact(id, "N", out _) is false))
			ThrowHelper.ThrowArgumentException(nameof(id), $"The given id ({id}) did not follow the expected formatting.");
	}
	#endregion

	#region Methods
	[ExcludeFromCodeCoverage]
	private string DebuggerDisplay()
	{
		if (ParentId is not null)
			return $"{nameof(Universe)} {{ Id = ({Id}), Parent = ({ParentId}), Name = ({Name}) }}";

		return $"{nameof(Universe)} {{ Id = ({Id}), Name = ({Name}) }}";
	}
	#endregion
}
