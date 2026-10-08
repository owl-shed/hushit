namespace OwlShed.Hushit.Data.EFCore.Images.Kinds;

/// <summary>
/// Represents the database model for image kinds.
/// </summary>
[Table("image_kinds")]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public sealed class ImageKind
{
	#region Constants
	/// <summary>The special id for front covers.</summary>
	public const string FrontCover = "front_cover";

	/// <summary>The special id for back covers.</summary>
	public const string BackCover = "back_cover";
	#endregion

	#region Properties
	/// <summary>The id of the image kind.</summary>
	[Column(Order = 0)]
	public string Id { get; init; }

	/// <summary>The description for an unknown image kind.</summary>
	[Column(Order = 1)]
	public string? UnknownKindDescription { get; init; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="ImageKind"/>.</summary>
	/// <param name="id">The id of the image kind.</param>
	/// <param name="unknownKindDescription">The id of the unknown image kind.</param>
	public ImageKind(string id, string? unknownKindDescription = null)
	{
		Id = id;
		UnknownKindDescription = unknownKindDescription;
	}
	#endregion

	#region Methods
	[ExcludeFromCodeCoverage]
	private string DebuggerDisplay()
	{
		if (UnknownKindDescription is not null)
			return $"{nameof(ImageKind)} {{ Id = ({Id}), Unknown = ({UnknownKindDescription}) }}";

		return $"{nameof(ImageKind)} {{ Id = ({Id}) }}";
	}
	#endregion
}
