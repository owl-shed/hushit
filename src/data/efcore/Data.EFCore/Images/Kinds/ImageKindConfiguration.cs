namespace OwlShed.Hushit.Data.EFCore.Images.Kinds;

/// <summary>
/// Represents the entity type configuration for the <see cref="ImageKind"/>.
/// </summary>
public sealed class ImageKindConfiguration : IEntityTypeConfiguration<ImageKind>
{
	#region Methods
	/// <inheritdoc/>
	public void Configure(EntityTypeBuilder<ImageKind> builder)
	{
		builder
			.HasIndex(nameof(ImageKind.UnknownKindDescription))
			.HasDatabaseName("ix_image_kind_unknown_kind_description_unique_name")
			.IsUnique();

		builder.HasData([
			new(ImageKind.FrontCover),
			new(ImageKind.BackCover),
		]);
	}
	#endregion
}
