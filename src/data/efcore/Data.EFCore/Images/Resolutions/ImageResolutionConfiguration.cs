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
		builder.HasKey(c => new { c.ImageId, c.Width, c.Height });
	}
	#endregion
}
