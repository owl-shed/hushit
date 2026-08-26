namespace OwlShed.Hushit.Data;

/// <summary>
/// 	Represents an access point to all of the data that Hushit stores and manages.
/// </summary>
public interface IHushitData
{
	#region Properties
	/// <summary>The data repository for audio file information.</summary>
	IAudioFileRepository AudioFiles { get; }

	/// <summary>The data repository for artist information.</summary>
	IArtistRepository Artists { get; }

	/// <summary>The data repository for album information.</summary>
	IAlbumRepository Albums { get; }

	/// <summary>The data repository for track information.</summary>
	ITrackRepository Tracks { get; }

	/// <summary>The data repository for genre information.</summary>
	IGenreRepository Genres { get; }
	#endregion
}

/// <inheritdoc cref="IHushitData"/>
public sealed class HushitData : IHushitData
{
	#region Properties
	/// <inheritdoc/>
	public IAudioFileRepository AudioFiles { get; }

	/// <inheritdoc/>
	public IArtistRepository Artists => throw new NotImplementedException();

	/// <inheritdoc/>
	public IAlbumRepository Albums => throw new NotImplementedException();

	/// <inheritdoc/>
	public ITrackRepository Tracks => throw new NotImplementedException();

	/// <inheritdoc/>
	public IGenreRepository Genres => throw new NotImplementedException();
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="HushitData"/> instance.</summary>
	/// <param name="directory">The base directory to store the Hushit data in.</param>
	public HushitData(string directory)
	{
		string audioFileDirectory = Path.Combine(directory, "audio_files");
		AudioFiles = new AudioFileRepository(this, audioFileDirectory);
	}
	#endregion
}
