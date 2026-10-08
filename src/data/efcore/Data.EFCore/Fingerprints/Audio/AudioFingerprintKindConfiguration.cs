namespace OwlShed.Hushit.Data.EFCore.Fingerprints.Audio;

/// <summary>
/// Represents the entity type configuration for the <see cref="AudioFingerprintKind"/>.
/// </summary>
public sealed class AudioFingerprintKindConfiguration : IEntityTypeConfiguration<AudioFingerprintKind>
{
	#region Methods
	/// <inheritdoc/>
	public void Configure(EntityTypeBuilder<AudioFingerprintKind> builder)
	{
		builder.HasData([
			new(AudioFingerprintKind.Chromaprint),
		]);
	}
	#endregion
}
