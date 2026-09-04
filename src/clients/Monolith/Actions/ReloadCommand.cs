namespace OwlShed.Hushit.Monolith.Actions;

internal sealed class ReloadCommand : Command
{
	#region Constructors
	public ReloadCommand() : base("reload", "Reloads the metadata for the already imported audio files.")
	{
		Option<bool> forceOption = new("force", "-f", "--force")
		{
			Description = "Whether to forcefully reload the metadata, even if the audio file hasn't changed.",
			DefaultValueFactory = (parse) => false,
		};

		Option<int> concurrencyOption = new("concurrency", "-c", "--concurrency")
		{
			Description = "The degree of concurrency to use when reloading the audio files.",
			DefaultValueFactory = (argument) => Environment.ProcessorCount
		};

		Add(forceOption);
		Add(concurrencyOption);

		SetAction(async (parse, cancellation) =>
		{
			// Todo(Nightowl): This is an assumption for now, just for making debugging easier;
			string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			string directory = Path.Combine(home, ".local/share/org.owlshed.hushit/data");

			HushitData data = new(directory);

			Console.Error.WriteLine($"Reloading existing audio files.");

			bool force = parse.GetRequiredValue(forceOption);

			Stopwatch stopwatch = Stopwatch.StartNew();
			ImportSettings settings = new()
			{
				Concurrency = parse.GetRequiredValue(concurrencyOption),
				Force = force
			};

			await data.ReloadAsync(settings, cancellation).ConfigureAwait(false);
			stopwatch.Stop();

			if (stopwatch.Elapsed.Minutes >= 1)
				Console.WriteLine($"Reloaded the audio files in {(int)stopwatch.Elapsed.TotalMinutes:n0}m {stopwatch.Elapsed.Seconds}.{stopwatch.Elapsed.Milliseconds}s.");
			else
				Console.WriteLine($"Reloaded the audio files in {stopwatch.Elapsed.TotalSeconds:n3}s.");
		});
	}
	#endregion
}
