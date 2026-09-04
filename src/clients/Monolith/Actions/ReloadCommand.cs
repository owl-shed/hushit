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

			int concurrency = parse.GetRequiredValue(concurrencyOption);
			bool force = parse.GetRequiredValue(forceOption);

			await ImportHelper.ReloadAsync(data, concurrency, force, cancellation).ConfigureAwait(false);
		});
	}
	#endregion
}
