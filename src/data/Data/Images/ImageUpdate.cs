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

	/// <inheritdoc cref="IAudioFileReferencesInfo.AudioFileIds"/>
	public ListUpdate<string> AudioFileIds { get; set; } = new();
	#endregion
}
