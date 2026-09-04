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

			string[] files = parse.GetRequiredValue(filesArgument);
			int concurrency = parse.GetRequiredValue(concurrencyOption);
			bool force = parse.GetRequiredValue(forceOption);

			await ImportHelper.ImportAsync(data, files, concurrency, force, cancellation).ConfigureAwait(false);
		});
	}
	#endregion
}
