namespace OwlShed.Hushit.Data.EFCore;

/// <summary>
/// Represents a factory for the <see cref="DataContext"/>.
/// </summary>
public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
	#region Methods
	/// <inheritdoc/>
	public DataContext CreateDbContext(string[] args)
	{
		DbContextOptionsBuilder<DataContext> builder = new();

		string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		path = Path.Combine(path, "org.owlshed.hushit", "dev");

		Directory.CreateDirectory(path);
		path = Path.Combine(path, "data.db");

		builder.UseSqlite($"Data Source='{path}'");

		return new(builder.Options);
	}
	#endregion
}
