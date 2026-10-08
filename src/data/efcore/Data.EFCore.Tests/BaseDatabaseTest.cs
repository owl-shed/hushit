using System.IO;
using Microsoft.Data.Sqlite;

namespace OwlShed.Hushit.Data.EFCore.Tests;

public abstract class BaseDatabaseTest : IDisposable
{
	#region Fields
	private readonly SqliteConnection _connection;
	private readonly DbContextOptions<DataContext> _contextOptions;
	#endregion

#if DEBUG
	protected string DatabaseName { get; }
#endif

	#region Constructors
	public BaseDatabaseTest()
	{
#if DEBUG
		string directory = Path.Combine(DatabaseTestAssemblySetup.DatabaseDirectoryName, GetType().Name);
		string file = $"{Guid.NewGuid():N}.db";
		Directory.CreateDirectory(directory);

		DatabaseName = Path.Combine(directory, file);
		_connection = new($"Filename={DatabaseName}");
#else
		_connection = new("Filename=:memory:");
#endif

		_connection.Open();

		_contextOptions = new DbContextOptionsBuilder<DataContext>()
			.UseSqlite(_connection)
			.LogTo(Console.WriteLine)
			.Options;

		using (DataContext context = CreateContext())
		{
			context.Database.EnsureCreated();
			context.SaveChanges();
		}
	}
	#endregion

	#region Methods
	public DataContext CreateContext() => new(_contextOptions);

	public void Dispose()
	{
		_connection.Dispose();
	}
	#endregion
}

#if DEBUG
[TestClass]
public static class DatabaseTestAssemblySetup
{
	#region Constructors
	public const string DatabaseDirectoryName = "test_databases";
	#endregion

	#region Functions
	[AssemblyInitialize]
	public static void AssemblySetup(TestContext context)
	{
		if (Directory.Exists(DatabaseDirectoryName))
			Directory.Delete(DatabaseDirectoryName, true);

		Directory.CreateDirectory(DatabaseDirectoryName);
	}
	#endregion
}
#endif
