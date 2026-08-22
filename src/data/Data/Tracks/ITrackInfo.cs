namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents information about a track.
/// </summary>
public interface ITrackInfo : IDataModel<ITrackUpdate>, IArtistReferencesInfo, IGenreReferencesInfo
{
	#region Properties
	/// <summary>The name of the track.</summary>
	string Name { get; }

	/// <summary>The duration of the track.</summary>
	TimeSpan Duration { get; }

	/// <summary>The id of the album that the track belongs to.</summary>
	string? AlbumId { get; }

	/// <summary>The id of the audio file that the track is linked to.</summary>
	string? AudioFileId { get; }
	#endregion
}
