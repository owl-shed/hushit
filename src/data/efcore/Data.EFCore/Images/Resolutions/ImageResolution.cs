namespace OwlShed.Hushit.Data.EFCore.Images.Resolutions;

/// <summary>
/// Represents the database model for image resolutions.
/// </summary>
[Table("image_resolution")]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public sealed class ImageResolution
{
	#region Properties
	/// <summary>The id of the image resolution.</summary>
	[Column(Order = 0)]
	public int Id { get; init; }

	/// <summary>The id of the image that the resolution is for.</summary>
	[Column(Order = 1)]
	public string ImageId { get; init; }

	/// <summary>The width of the image resolution.</summary>
	[Column(Order = 2)]
	public int Width { get; init; }

	/// <summary>The height of the image resolution.</summary>
	[Column(Order = 2)]
	public int Height { get; init; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="ImageResolution"/>.</summary>
	/// <param name="imageId">The id of the image that the resolution is for.</param>
	/// <param name="width">The width of the image resolution.</param>
	/// <param name="height">The height of the image resolution.</param>
	public ImageResolution(string imageId, int width, int height)
	{
		ImageId = imageId;
		Width = width;
		Height = height;
	}
	#endregion

	#region Methods
	[ExcludeFromCodeCoverage]
	private string DebuggerDisplay()
	{
		return $"{nameof(ImageResolution)} {{ ImageId = ({ImageId}), Resolution = ({Width}x{Height}) }}";
	}
	#endregion
}
