namespace OwlShed.Hushit.Data.Images;

/// <summary>
/// 	Represents information about an image.
/// </summary>
public interface IImageInfo : IDataModel<MutableImage, ImageUpdate>, IAudioFileReferencesInfo
{
	#region Properties
	/// <summary>The path to the image file.</summary>
	string Path { get; }

	/// <summary>The hash of the image file.</summary>
	HashInfo Hash { get; }
	#endregion
}
