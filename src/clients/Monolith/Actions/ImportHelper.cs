namespace OwlShed.Hushit.Monolith.Actions;

internal static class ImportHelper
{
	#region Functions
	public static async ValueTask ImportAsync(
		IHushitData data,
		IReadOnlyCollection<string> files,
		int concurrency,
		bool force,
		CancellationToken cancellation = default)
	{
		string preMessage = (files.Count, force) switch
		{
			(1, true) => $"Importing 1 audio file forcefully.",
			(_, true) => $"Importing {files.Count:n0} audio files forcefully.",
			(1, false) => $"Importing 1 audio file.",
			(_, false) => $"Importing {files.Count:n0} audio files."
		};

		AnsiConsole.MarkupLineInterpolated($"[italic]{preMessage}[/]");
		Stopwatch stopwatch = Stopwatch.StartNew();

		async ValueTask CallbackAsync(ImportSettings settings, CancellationToken cancellation)
		{
			await data.ImportAsync(files, settings, cancellation).ConfigureAwait(false);
		}
		ImportProgress progress = await StartAsync(CallbackAsync, concurrency, force, cancellation).ConfigureAwait(false);

		stopwatch.Stop();
		AnsiConsole.MarkupLineInterpolated($"[italic]Imported the requested audio files in {stopwatch.Elapsed.ToPrettyTime()}[/].");

		if (progress is not null && progress.Warnings.Any())
		{
			AnsiConsole.WriteLine();
			AnsiConsole.MarkupLine("[DarkOrange bold]Import warnings.[/]");
			foreach (string warning in progress.Warnings)
				AnsiConsole.MarkupLineInterpolated($"[DarkOrange]{warning}[/]");
		}
	}
	public static async ValueTask ReloadAsync(IHushitData data, int concurrency, bool force, CancellationToken cancellation = default)
	{
		AnsiConsole.MarkupLine($"[italic]Reloading existing audio files.[/]");
		Stopwatch stopwatch = Stopwatch.StartNew();

		async ValueTask CallbackAsync(ImportSettings settings, CancellationToken cancellation)
		{
			await data.ReloadAsync(settings, cancellation).ConfigureAwait(false);
		}
		ImportProgress progress = await StartAsync(CallbackAsync, concurrency, force, cancellation).ConfigureAwait(false);

		stopwatch.Stop();
		AnsiConsole.MarkupLineInterpolated($"[italic]Reloaded the existing audio files in {stopwatch.Elapsed.ToPrettyTime()}[/].");

		if (progress is not null && progress.Warnings.Any())
		{
			AnsiConsole.WriteLine();
			AnsiConsole.MarkupLine("[DarkOrange bold]Reload warnings.[/]");
			foreach (string warning in progress.Warnings)
				AnsiConsole.MarkupLineInterpolated($"[DarkOrange]{warning}[/]");
		}
	}
	private static async ValueTask<ImportProgress> StartAsync(Func<ImportSettings, CancellationToken, ValueTask> callback, int concurrency, bool force, CancellationToken cancellation)
	{
		Progress progressUI = AnsiConsole.Progress().Columns(
					new TaskDescriptionColumn(),
					new ProgressBarColumn(),
					new PercentageColumn(),
					new ElapsedTimeColumn(),
					new SpinnerColumn()
				);

		ImportProgress? lastProgress = null;

		await progressUI.StartAsync(async ctx =>
		{
			Dictionary<ImportStage, ProgressTask> tasks = [];
			foreach (ImportStage stage in Enum.GetValues<ImportStage>())
			{
				ProgressTask task = ctx.AddTask(stage.NaturalName, autoStart: false);
				tasks.Add(stage, task);
			}

			ImportStage? lastStage = null;

			void Update(ImportProgress progress)
			{
				lastProgress = progress;

				ProgressTask task = tasks[progress.Stage];
				task.MaxValue = progress.TotalSteps;
				task.Value = progress.CurrentSteps;

				if (progress.TotalSteps is 0)
				{
					task.MaxValue = 1;
					task.Value = 1;
				}

				if (task.IsStarted is false)
					task.StartTask();

				if (lastStage is not null && lastStage != progress.Stage)
					tasks[lastStage.Value].StopTask();

				lastStage = progress.Stage;
			}

			ImportSettings settings = new()
			{
				Concurrency = concurrency,
				Force = force,
				Progress = new Progress<ImportProgress>(Update)
			};

			await callback.Invoke(settings, cancellation).ConfigureAwait(false);
		});

		return lastProgress ?? new();
	}
	#endregion
}
