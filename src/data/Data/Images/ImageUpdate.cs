namespace OwlShed.Hushit.Data.Images;

/// <summary>
/// 	Represents an update to the <see cref="IImageInfo"/>.
/// </summary>

public sealed class ImageUpdate
{
	#region Properties
	/// <inheritdoc cref="IImageInfo.Path"/>
	public ValueUpdate<string> Path { get; set; }

	/// <inheritdoc cref="IImageInfo.Hash"/>
	public ValueUpdate<HashInfo> Hash { get; set; }
	#endregion
}
