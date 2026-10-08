namespace OwlShed.Hushit.Data.EFCore.Hashes;

/// <summary>
/// Represents the entity type configuration for the <see cref="HashKind"/>.
/// </summary>
public sealed class HashKindConfiguration : IEntityTypeConfiguration<HashKind>
{
	#region Methods
	/// <inheritdoc/>
	public void Configure(EntityTypeBuilder<HashKind> builder)
	{
		builder.HasData([
			new(HashKind.Sha256),
		]);
	}
	#endregion
}
