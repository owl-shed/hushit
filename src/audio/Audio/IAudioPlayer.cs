using System.IO;

namespace OwlShed.Hushit.Audio;

/// <summary>
/// 	Represents a general audio player.
/// </summary>
public interface IAudioPlayer : INotifyPropertyChanged, IDisposable
{
	#region Properties
	/// <summary>The master volume of the audio player.</summary>
	/// <remarks>This value should be between <c>0</c> and <c>1</c>.</remarks>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if the given value is not between <c>0</c> and <c>1</c>.</exception>
	double Volume { get; set; }

	/// <summary>Whether the audio playback is currently muted.</summary>
	/// <remarks>
	/// 	This should be treated as an override to the <see cref="Volume"/>, and it
	/// 	should not influence, or be influence by the <see cref="Volume"/> value.
	/// </remarks>
	bool IsMute { get; set; }

	/// <summary>Whether the audio is currently playing.</summary>
	bool IsPlaying { get; set; }

	/// <summary>The duration of the loaded audio file.</summary>
	TimeSpan Duration { get; }

	/// <summary>The current position in the audio file.</summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// 	Thrown if the given value is either less than <see cref="TimeSpan.Zero"/>,
	/// 	or it's greater than the <see cref="Duration"/>.
	/// </exception>
	TimeSpan Position { get; set; }
	#endregion

	#region Methods
	/// <summary>Loads the audio from the file at the given <paramref name="path"/> and prepares it for playback.</summary>
	/// <param name="path">The file path to load the audio from.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <exception cref="FileNotFoundException">Thrown if the file at the given <paramref name="path"/> doesn't exist.</exception>
	/// <exception cref="NotSupportedException">Thrown if the audio format is not supported.</exception>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask LoadAsync(string path, CancellationToken cancellation = default);
	#endregion
}
