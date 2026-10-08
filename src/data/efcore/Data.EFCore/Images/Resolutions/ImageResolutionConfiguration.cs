namespace OwlShed.Hushit.Data.EFCore.Images.Resolutions;

/// <summary>
/// Represents the entity type configuration for the <see cref="ImageResolution"/>.
/// </summary>
public sealed class ImageResolutionConfiguration : IEntityTypeConfiguration<ImageResolution>
{
	#region Methods
	/// <inheritdoc/>
	public void Configure(EntityTypeBuilder<ImageResolution> builder)
	{
		builder
			.HasIndex(nameof(ImageResolution.ImageId), nameof(ImageResolution.Width), nameof(ImageResolution.Height))
			.HasDatabaseName("ix_image_resolution_unique_resolution")
			.IsUnique();
	}
	#endregion
}
