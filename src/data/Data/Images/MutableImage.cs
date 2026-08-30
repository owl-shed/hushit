namespace OwlShed.Hushit.Data.Images;

/// <summary>
/// 	Represents a mutable version of <see cref="IImageInfo"/>.
/// </summary>
public sealed class MutableImage : MutableDataModelBase<MutableImage, ImageUpdate>
{
	#region Properties
	/// <inheritdoc cref="IImageInfo.Path"/>
	public string? Path { get; set; }

	/// <inheritdoc cref="IImageInfo.Hash"/>
	public HashInfo? Hash { get; set; }

	/// <inheritdoc cref="IAudioFileReferencesInfo.AudioFileIds"/>
	public IList<string> AudioFileIds { get; set; } = [];
	#endregion

	#region Methods
	/// <inheritdoc/>
	public override ImageUpdate GetUpdateFrom(MutableImage oldState)
	{
		if (Path is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Path)}' to have a value.");

		if (Hash is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Hash)}' to have a value.");

		return new()
		{
			Path = Update.Value(oldState.Path, Path),
			Hash = oldState.Hash is null ? Hash.Value : Update.Value(oldState.Hash, Hash),
			AudioFileIds = Update.List(oldState.AudioFileIds, AudioFileIds),
		};
	}
	#endregion
}
