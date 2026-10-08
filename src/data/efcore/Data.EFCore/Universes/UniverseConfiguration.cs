namespace OwlShed.Hushit.Data.EFCore.Universes;

/// <summary>
/// Represents the entity type configuration for the <see cref="Universe"/>.
/// </summary>
public sealed class UniverseConfiguration : IEntityTypeConfiguration<Universe>
{
	#region Methods
	/// <inheritdoc/>
	public void Configure(EntityTypeBuilder<Universe> builder)
	{
		builder
			.HasMany<Universe>()
			.WithOne()
			.HasForeignKey(e => e.ParentId)
			.IsRequired(false);

		builder
			.HasIndex(nameof(Universe.Name))
			.HasDatabaseName("ix_universe_unique_name")
			.IsUnique();

		builder.HasData([
			new(Universe.RealId, parentId: null, "Real"),
		]);
	}
	#endregion
}
