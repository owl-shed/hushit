namespace OwlShed.Hushit.Data.AudioFiles;

/// <summary>
/// 	Represents an update to the <see cref="IAudioFileInfo"/>.
/// </summary>
public sealed class AudioFileUpdate
{
	#region Properties
	/// <inheritdoc cref="IAudioFileInfo.Path"/>
	public ValueUpdate<string> Path { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.Hash"/>
	public ValueUpdate<HashInfo> Hash { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackName"/>
	public ValueUpdate<string> TrackName { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackId"/>
	public NullableUpdate<string> TrackId { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackDate"/>
	public NullableUpdate<DateInfo?> TrackDate { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.AlbumName"/>
	public NullableUpdate<string> AlbumName { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.AlbumDate"/>
	public NullableUpdate<DateInfo?> AlbumDate { get; set; }

	/// <inheritdoc cref="IAudioFileInfo.TrackArtists"/>
	public ListUpdate<string> TrackArtists { get; set; } = new();

	/// <inheritdoc cref="IAudioFileInfo.AlbumArtists"/>
	public ListUpdate<string> AlbumArtists { get; set; } = new();

	/// <inheritdoc cref="IAudioFileInfo.TrackGenres"/>
	public ListUpdate<string> TrackGenres { get; set; } = new();

	/// <inheritdoc cref="IAudioFileInfo.AlbumGenres"/>
	public ListUpdate<string> AlbumGenres { get; set; } = new();
	#endregion
}
