#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Add the empty database.")]
	public partial class AddEmptyDatabase : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder) { }

		/// <inheritdoc />
		[ExcludeFromCodeCoverage(Justification = "Literally does nothing and I don't see a way to easily undo this migration at runtime.")]
		protected override void Down(MigrationBuilder migrationBuilder) { }
	}
}
