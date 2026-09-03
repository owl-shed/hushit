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

	/// <summary>The data repository for image file information.</summary>
	IImageRepository Images { get; }
	#endregion
}

/// <inheritdoc cref="IHushitData"/>
public sealed class HushitData : IHushitData
{
	#region Properties
	/// <inheritdoc/>
	public IAudioFileRepository AudioFiles { get; }

	/// <inheritdoc/>
	public IArtistRepository Artists { get; }

	/// <inheritdoc/>
	public IAlbumRepository Albums { get; }

	/// <inheritdoc/>
	public ITrackRepository Tracks => throw new NotImplementedException();

	/// <inheritdoc/>
	public IGenreRepository Genres => throw new NotImplementedException();

	/// <inheritdoc/>
	public IImageRepository Images { get; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="HushitData"/> instance.</summary>
	/// <param name="directory">The base directory to store the Hushit data in.</param>
	public HushitData(string directory)
	{
		string audioFileDirectory = Path.Combine(directory, "audio_files");
		AudioFiles = new AudioFileRepository(this, audioFileDirectory);

		string imageDirectory = Path.Combine(directory, "images");
		Images = new ImageRepository(this, imageDirectory);

		string artistDirectory = Path.Combine(directory, "artists");
		Artists = new ArtistRepository(this, artistDirectory);

		string albumDirectory = Path.Combine(directory, "albums");
		Albums = new AlbumRepository(this, albumDirectory);

		AudioFiles.Initialise();
		Images.Initialise();
		Artists.Initialise();
		Albums.Initialise();
	}
	#endregion
}
