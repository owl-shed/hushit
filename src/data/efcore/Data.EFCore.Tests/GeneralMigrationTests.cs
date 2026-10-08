using Microsoft.Data.Sqlite;

namespace OwlShed.Hushit.Data.EFCore.Tests;

[TestClass]
public sealed class GeneralMigrationTests
{
	#region Tests
	[TestMethod]
	public void MigrateUp_NoExceptions()
	{
		// Arrange
		SqliteConnection connection = new("Filename=:memory:");
		connection.Open();

		DbContextOptions<DataContext> contextOptions = new DbContextOptionsBuilder<DataContext>()
			.UseSqlite(connection)
			.Options;

		using (DataContext context = new(contextOptions))
		{
			// Act & Assert
			context.Database.Migrate();
		}
	}

	[TestMethod]
	public void MigrateDown_NoExceptions()
	{
		// Arrange
		SqliteConnection connection = new("Filename=:memory:");
		connection.Open();

		DbContextOptions<DataContext> contextOptions = new DbContextOptionsBuilder<DataContext>()
			.UseSqlite(connection)
			.Options;

		using (DataContext context = new(contextOptions))
		{
			context.Database.Migrate();
			string firstMigrationId = context.Database.GetAppliedMigrations().First();

			// Act & Assert
			context.Database.Migrate(firstMigrationId);
		}
	}
	#endregion
}
