namespace OwlShed.Hushit.Data;

/// <summary>
/// 	Represents the stage of a data import operation.
/// </summary>
public enum ImportStage
{
	/// <summary>The stage of importing audio files.</summary>
	AudioFiles,

	/// <summary>The stage of checking for missing genres.</summary>
	Genres,

	/// <summary>The stage of checking for missing artists.</summary>
	Artists,

	/// <summary>The stage of checking for missing albums.</summary>
	Albums,

	/// <summary>The stage of checking for missing tracks.</summary>
	Tracks
}

/// <summary>
/// 	Contains various extensions related to the <see cref="ImportStage"/>.
/// </summary>
public static class ImportStageExtensions
{
	extension(ImportStage stage)
	{
		#region Properties
		/// <summary>Gets the name of the stage as a natural piece of text.</summary>
		public string NaturalName
		{
			get
			{
				return stage switch
				{
					ImportStage.AudioFiles => "Audio files",
					ImportStage.Genres => "Genres",
					ImportStage.Artists => "Artists",
					ImportStage.Albums => "Albums",
					ImportStage.Tracks => "Tracks",

					_ => ThrowHelper.ThrowInvalidOperationException<string>($"Unknown import stage ({stage}).")
				};
			}
		}
		#endregion
	}
}

/// <summary>
/// 	Represents the progress of an import operation.
/// </summary>
public sealed class ImportProgress
{
	#region Fields
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int _currentSteps;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly List<string> _warnings = [], _messages = [];
	private readonly ReaderWriterLockSlim _lock = new();
	#endregion

	#region Properties
	/// <summary>The warnings emitted during the import operation.</summary>
	public IReadOnlyList<string> Warnings
	{
		get
		{
			using (_lock.ReadLock())
				return _warnings.ToArray();
		}
	}

	/// <summary>The messages emitted during the import operation.</summary>
	public IReadOnlyList<string> Messages
	{
		get
		{
			using (_lock.ReadLock())
				return _messages.ToArray();
		}
	}

	/// <summary>The current stage of importing.</summary>
	public ImportStage Stage { get; set; }

	/// <summary>The total amount of steps in the current stage.</summary>
	public int TotalSteps { get; set; }

	/// <summary>The current amount of steps in the current stage.</summary>
	public int CurrentSteps { get => _currentSteps; set => _currentSteps = value; }
	#endregion

	#region Methods
	/// <summary>Adds a new general message.</summary>
	/// <param name="message">The general message to add.</param>
	public void AddMessage(string message)
	{
		using (_lock.WriteLock())
			_messages.Add(message);
	}

	/// <summary>Adds a new warning message.</summary>
	/// <param name="warning">The warning message to add.</param>
	public void AddWarning(string warning)
	{
		using (_lock.WriteLock())
			_warnings.Add(warning);
	}

	/// <summary>Atomically increments the current step counter.</summary>
	public void IncrementStep() => Interlocked.Increment(ref _currentSteps);

	/// <summary>Sets the information for a new stage.</summary>
	/// <param name="stage">The new steps.</param>
	/// <param name="totalSteps">The total amount of steps in the new stage.</param>
	public void NewStage(ImportStage stage, int totalSteps)
	{
		Stage = stage;
		TotalSteps = totalSteps;
		CurrentSteps = 0;
	}
	#endregion
}
