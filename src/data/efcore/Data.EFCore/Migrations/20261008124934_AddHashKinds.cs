#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Add the hash kinds table.")]
	public partial class AddHashKinds : Migration
	{
		#region Methods
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "hash_kinds",
				columns: table => new
				{
					id = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_hash_kinds", x => x.id);
				});

			migrationBuilder.InsertData(
				table: "hash_kinds",
				column: "id",
				value: "sha256");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "hash_kinds");
		}
		#endregion
	}
}
