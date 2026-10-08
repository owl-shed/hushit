using Microsoft.EntityFrameworkCore.Sqlite.Migrations.Internal;

namespace OwlShed.Hushit.Data.EFCore;

/// <summary>
/// Represents the database context for accessing Hushit data.
/// </summary>
public sealed class DataContext : DbContext
{
	#region Nested types
	[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "Seems to be required for configuring the migration history table.")]
	private sealed class HushitHistoryRepository : SqliteHistoryRepository
	{
		#region Constructors
		public HushitHistoryRepository(HistoryRepositoryDependencies dependencies) : base(dependencies)
		{
		}
		#endregion

		#region Methods
		protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
		{
			base.ConfigureTable(history);

			history.Property(h => h.MigrationId).HasColumnName("migration_id");
			history.Property(h => h.ProductVersion).HasColumnName("efcore_version");
		}
		#endregion
	}
	#endregion

	#region Constructors
	/// <summary>Creates a new <see cref="DataContext"/>.</summary>
	/// <param name="options">The options used to customise the database context.</param>
	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{
		ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
	}
	#endregion

	#region Methods
	/// <inheritdoc/>
	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		options
			.UseSqlite(sqlite => sqlite.MigrationsHistoryTable("migration_history"))
			.ReplaceService<IHistoryRepository, HushitHistoryRepository>()
			.UseSnakeCaseNamingConvention();
	}

	/// <inheritdoc/>
	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
	}
	#endregion
}
