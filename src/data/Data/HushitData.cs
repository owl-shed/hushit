using System.Collections.Concurrent;

namespace OwlShed.Hushit.Data;

/// <inheritdoc cref="IHushitData"/>
public sealed class HushitData : IHushitData
{
	#region Nested types
	private sealed class ProgressInfo(ImportProgress progress, IProgress<ImportProgress>? reporter)
	{
		#region Properties
		public ImportProgress Progress { get; } = progress;
		public IProgress<ImportProgress>? Reporter { get; } = reporter;
		#endregion

		#region Constructors
		public ProgressInfo(IProgress<ImportProgress>? reporter) : this(new(), reporter) { }
		#endregion

		#region Methods
		public void NewStage(ImportStage stage, int totalSteps)
		{
			Progress.NewStage(stage, totalSteps);
			Report();
		}
		public void AddMessage(string message)
		{
			Progress.AddMessage(message);
			Report();
		}
		public void AddWarning(string message)
		{
			Progress.AddWarning(message);
			Report();
		}
		public void Increment()
		{
			Progress.IncrementStep();
			Report();
		}
		#endregion

		#region Helpers
		private void Report() => Reporter?.Report(Progress);
		#endregion
	}
	#endregion

	#region Properties
	/// <inheritdoc/>
	public IAudioFileRepository AudioFiles { get; }

	/// <inheritdoc/>
	public IArtistRepository Artists { get; }

	/// <inheritdoc/>
	public IAlbumRepository Albums { get; }

	/// <inheritdoc/>
	public ITrackRepository Tracks { get; }

	/// <inheritdoc/>
	public IGenreRepository Genres { get; }

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

		string trackDirectory = Path.Combine(directory, "tracks");
		Tracks = new TrackRepository(this, trackDirectory);

		string genreDirectory = Path.Combine(directory, "genres");
		Genres = new GenreRepository(this, genreDirectory);

		AudioFiles.Initialise();
		Images.Initialise();
		Artists.Initialise();
		Albums.Initialise();
		Tracks.Initialise();
		Genres.Initialise();
	}
	#endregion

	#region Methods
	/// <inheritdoc/>
	public async ValueTask ImportAsync(IReadOnlyCollection<string> files, ImportSettings settings, CancellationToken cancellation = default)
	{
		ParallelOptions options = new()
		{
			MaxDegreeOfParallelism = settings.Concurrency,
			CancellationToken = cancellation
		};

		ProgressInfo progress = new(settings.Progress);

		progress.NewStage(ImportStage.AudioFiles, files.Count);

		ConcurrentBag<IAudioFileInfo> audioFiles = [];
		await Parallel.ForEachAsync(files, options, async (file, cancellation) =>
		{
			progress.AddMessage($"Importing audio file: {file}");
			progress.Increment();

			IAudioFileInfo audioFile = await AudioFiles.GetOrCreateAsync(file, force: settings.Force, cancellation).ConfigureAwait(false);
			audioFiles.Add(audioFile);

		}).ConfigureAwait(false);

		await UpdateFromFilesAsync(audioFiles, settings, progress, cancellation).ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public async ValueTask ReloadAsync(ImportSettings settings, CancellationToken cancellation = default)
	{
		ParallelOptions options = new()
		{
			MaxDegreeOfParallelism = settings.Concurrency,
			CancellationToken = cancellation
		};

		ProgressInfo progress = new(settings.Progress);

		IAudioFileInfo[] allFiles = await AudioFiles.GetAllAsync(cancellation).ToArrayAsync(cancellation).ConfigureAwait(false);
		progress.NewStage(ImportStage.AudioFiles, allFiles.Length);

		ConcurrentBag<IAudioFileInfo> audioFiles = [];
		await Parallel.ForEachAsync(allFiles, options, async (file, cancellation) =>
		{
			progress.AddMessage($"Reloading audio file: {file}");
			progress.Increment();

			bool changed = settings.Force switch
			{
				true => await file.ReloadAsync(cancellation).ConfigureAwait(false),
				false => await file.TryReloadAsync(cancellation).ConfigureAwait(false)
			};

			if (changed)
				audioFiles.Add(file);

		}).ConfigureAwait(false);

		await UpdateFromFilesAsync(audioFiles, settings, progress, cancellation).ConfigureAwait(false);
	}

	private async ValueTask UpdateFromFilesAsync(
		IReadOnlyCollection<IAudioFileInfo> files,
		ImportSettings settings, ProgressInfo progress,
		CancellationToken cancellation = default)
	{
		ParallelOptions options = new()
		{
			MaxDegreeOfParallelism = settings.Concurrency,
			CancellationToken = cancellation
		};

		string[] genreNames = files.SelectMany(file => file.TrackGenres.Concat(file.AlbumGenres)).Distinct().ToArray();
		progress.NewStage(ImportStage.Genres, genreNames.Length);

		await Parallel.ForEachAsync(genreNames, options, async (name, cancellation) =>
		{
			progress.AddMessage($"Checking genre: '{name}'");
			progress.Increment();

			await Genres.GetOrCreateAsync(name, cancellation).ConfigureAwait(false);
		}
		).ConfigureAwait(false);

		string[] artistNames = files.SelectMany(file => file.TrackArtists.Concat(file.AlbumArtists)).Distinct().ToArray();
		progress.NewStage(ImportStage.Artists, artistNames.Length);

		await Parallel.ForEachAsync(artistNames, options, async (name, cancellation) =>
		{
			progress.AddMessage($"Checking artist: '{name}'");
			progress.Increment();

			await Artists.GetOrCreateAsync(name, cancellation).ConfigureAwait(false);
		}).ConfigureAwait(false);

		HashSet<AlbumKey> albumKeys = [];
		await Parallel.ForEachAsync(files, options, async (file, cancellation) =>
		{
			AlbumKey? key = await GetAlbumKeyAsync(file, progress, cancellation).ConfigureAwait(false);
			if (key is null)
				return;

			lock (albumKeys)
				albumKeys.Add(key);

		}).ConfigureAwait(false);

		progress.NewStage(ImportStage.Albums, albumKeys.Count);
		await Parallel.ForEachAsync(albumKeys, options, async (album, cancellation) =>
		{
			progress.AddMessage($"Checking artist: '{album.Name}'");
			progress.Increment();

			await Albums.GetOrCreateAsync(album.Name, album.ArtistIds, cancellation).ConfigureAwait(false);
		}).ConfigureAwait(false);

		progress.NewStage(ImportStage.Tracks, files.Count);
		await Parallel.ForEachAsync(files, options, async (file, cancellation) =>
		{
			progress.AddMessage($"Checking track for: '{file.Path}'");
			progress.Increment();

			AlbumKey? albumKey = await GetAlbumKeyAsync(file, null, cancellation).ConfigureAwait(false);
			IAlbumInfo? album = albumKey is null ? null : await Albums.GetOrCreateAsync(albumKey.Name, albumKey.ArtistIds, cancellation).ConfigureAwait(false);

			List<string> trackArtistIds = [];
			foreach (string name in file.TrackArtists)
			{
				IArtistInfo artist = await Artists.GetOrCreateAsync(name, cancellation).ConfigureAwait(false);
				trackArtistIds.Add(artist.Id);
			}

			List<string> trackGenreIds = [];
			foreach (string name in file.TrackGenres)
			{
				IGenreInfo genre = await Genres.GetOrCreateAsync(name, cancellation).ConfigureAwait(false);
				trackGenreIds.Add(genre.Id);
			}

			void UpdateTrack(MutableTrack track)
			{
				track.Name = file.TrackName;
				track.Duration = file.Duration ?? TimeSpan.Zero;
				track.AlbumId = album?.Id;
				track.ArtistIds = trackArtistIds.ToArray();
				track.GenreIds = trackGenreIds.ToArray();
			}

			ITrackInfo? track;
			if (file.TrackId is not null)
			{
				track = await Tracks.TryGetAsync(file.TrackId, cancellation).ConfigureAwait(false);
				if (track is not null)
				{
					await Tracks.UpdateAsync(track, UpdateTrack, cancellation).ConfigureAwait(false);
					return;
				}
			}

			track = await Tracks.CreateAsync(UpdateTrack, cancellation).ConfigureAwait(false);
			await file.UpdateAsync(mutable => mutable.TrackId = track.Id, cancellation).ConfigureAwait(false);

		}).ConfigureAwait(false);

	}
	#endregion

	#region Helpers
	private async ValueTask<AlbumKey?> GetAlbumKeyAsync(IAudioFileInfo file, ProgressInfo? progress, CancellationToken cancellation = default)
	{
		if (file.AlbumName is null)
		{
			if (file.AlbumArtists.Any())
				progress?.AddWarning($"Audio file '{file.Path}' has album artists but no album name!");

			return null;
		}

		if (file.AlbumArtists.Count is 0)
		{
			progress?.AddWarning($"Audio file '{file.Path}' has an album name but no album artists.");
			return null;
		}

		List<string> albumArtistIds = [];
		foreach (string name in file.AlbumArtists)
		{
			IArtistInfo artist = await Artists.GetOrCreateAsync(name, cancellation).ConfigureAwait(false);
			albumArtistIds.Add(artist.Id);
		}

		return new(file.AlbumName, albumArtistIds);
	}
	#endregion
}
