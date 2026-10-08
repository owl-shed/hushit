namespace OwlShed.Hushit.Data.EFCore.Images;

/// <summary>
/// Represents the entity type configuration for the <see cref="Image"/>.
/// </summary>
public sealed class ImageConfiguration : IEntityTypeConfiguration<Image>
{
	#region Methods
	/// <inheritdoc/>
	public void Configure(EntityTypeBuilder<Image> builder)
	{
		builder
			.HasMany(e => e.Resolutions)
			.WithOne()
			.HasForeignKey(nameof(ImageResolution.ImageId))
			.IsRequired();

		builder
			.HasOne<ImageKind>()
			.WithMany()
			.HasForeignKey(e => e.KindId)
			.IsRequired();
	}
	#endregion
}
