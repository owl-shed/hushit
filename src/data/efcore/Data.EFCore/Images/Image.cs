namespace OwlShed.Hushit.Data.EFCore.Images;

/// <summary>
/// Represents the database model for images.
/// </summary>
[Table("images")]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public sealed class Image
{
	#region Properties
	/// <summary>The id of the image.</summary>
	[Column(Order = 0)]
	public string Id { get; init; }

	/// <summary>The id of the image kind.</summary>
	[Column(Order = 1)]
	public string KindId { get; init; }

	/// <summary>The original width of the image.</summary>
	[Column(Order = 2)]
	public int Width { get; init; }

	/// <summary>The original width of the image.</summary>
	[Column(Order = 3)]
	public int Height { get; init; }

	/// <summary>The available image resolutions, in addition to the original resolution.</summary>
	public ICollection<ImageResolution> Resolutions { get; init; } = [];
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="Image"/>.</summary>
	/// <param name="id">The id of the image.</param>
	/// <param name="kindId">The id of the image kind.</param>
	/// <param name="width">The original width of the image.</param>
	/// <param name="height">The original height of the image.</param>
	public Image(string id, string kindId, int width, int height)
	{
		Id = id;
		KindId = kindId;
		Width = width;
		Height = height;
	}
	#endregion

	#region Helpers
	private string DebuggerDisplay() => $"{nameof(Image)} {{ Id = ({Id}), Kind = ({KindId}), Resolution = ({Width}x{Height}) }}";
	#endregion
}
