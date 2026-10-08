namespace OwlShed.Hushit.Data.EFCore.Fingerprints.Audio;

/// <summary>
/// Represents the database model for audio fingerprint kinds.
/// </summary>
[Table("audio_fingerprint_kinds")]
[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(), nq}}")]
public sealed class AudioFingerprintKind
{
	#region Constants
	/// <summary>The special id for chromaprint.</summary>
	public const string Chromaprint = "chromaprint";
	#endregion

	#region Properties
	/// <summary>The id of the audio fingerprint kind.</summary>
	[Column(Order = 0)]
	public string Id { get; init; }
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="AudioFingerprintKind"/>.</summary>
	/// <param name="id">The id of the audio fingerprint kind.</param>
	public AudioFingerprintKind(string id) => Id = id;
	#endregion

	#region Methods
	[ExcludeFromCodeCoverage]
	private string DebuggerDisplay() => $"{nameof(AudioFingerprintKind)} {{ Id = ({Id}) }}";
	#endregion
}
