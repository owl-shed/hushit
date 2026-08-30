namespace OwlShed.Hushit.Monolith.Actions;

internal sealed class ImportCommand : Command
{
	#region Constructors
	public ImportCommand() : base("import", "Imports the specified audio files.")
	{
		Option<bool> forceOption = new("force", "-f", "--force")
		{
			Description = "Whether to forcefully reload the metadata if the audio file was already imported before.",
			DefaultValueFactory = (parse) => false,
		};

		Option<int> concurrencyOption = new("concurrency", "-c", "--concurrency")
		{
			Description = "The degree of concurrency to use when importing the audio files.",
			DefaultValueFactory = (argument) => Environment.ProcessorCount
		};

		Argument<string[]> filesArgument = new("files")
		{
			Description = "The audio files to import.",
			Arity = ArgumentArity.OneOrMore
		};

		filesArgument.AcceptLegalFilePathsOnly();

		Add(forceOption);
		Add(concurrencyOption);
		Add(filesArgument);

		SetAction(async (parse, cancellation) =>
		{
			// Todo(Nightowl): This is an assumption for now, just for making debugging easier;
			string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			string directory = Path.Combine(home, ".local/share/org.owlshed.hushit/data");

			HushitData data = new(directory);

			ParallelOptions options = new()
			{
				MaxDegreeOfParallelism = parse.GetValue(concurrencyOption),
				CancellationToken = cancellation
			};

			string[] files = parse.GetRequiredValue(filesArgument);

			if (files.Length is 1)
				Console.Error.WriteLine($"Importing 1 audio file.");
			else
				Console.Error.WriteLine($"Importing {files.Length:n0} audio files.");

			bool force = parse.GetValue(forceOption);

			Stopwatch stopwatch = Stopwatch.StartNew();
			await Parallel.ForEachAsync(files, options, async (file, cancellation) =>
			{
				Console.Error.WriteLine($"Importing {file}");
				await data.AudioFiles.CreateAsync(file, force, cancellation).ConfigureAwait(false);
			});

			stopwatch.Stop();

			if (stopwatch.Elapsed.TotalMinutes >= 1)
				Console.WriteLine($"Imported the requested audio files in {(int)stopwatch.Elapsed.TotalMinutes:n0}m {stopwatch.Elapsed.Seconds}.{stopwatch.Elapsed.Milliseconds}s.");
			else
				Console.WriteLine($"Imported the requested audio files in {stopwatch.Elapsed.TotalSeconds}s.");
		});
	}
	#endregion
}
