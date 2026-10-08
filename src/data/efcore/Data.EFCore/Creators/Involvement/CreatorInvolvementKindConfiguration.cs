namespace OwlShed.Hushit.Data.EFCore.Creators.Involvement;

/// <summary>
/// Represents the entity type configuration for the <see cref="CreatorInvolvementKind"/>.
/// </summary>
public sealed class CreatorInvolvementKindConfiguration : IEntityTypeConfiguration<CreatorInvolvementKind>
{
	#region Methods
	/// <inheritdoc/>
	public void Configure(EntityTypeBuilder<CreatorInvolvementKind> builder)
	{
		builder
			.HasIndex(nameof(CreatorInvolvementKind.Name))
			.HasDatabaseName("ix_creator_involvement_kind_unique_name")
			.IsUnique();

		builder.HasData([
			new(CreatorInvolvementKind.ArtistId, "Artist"),
			new(CreatorInvolvementKind.ComposerId, "Composer"),
			new(CreatorInvolvementKind.LyricistId, "Lyricist"),
		]);
	}
	#endregion
}
