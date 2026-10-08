#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Add the universes table.")]
	public partial class AddUniverses : Migration
	{
		#region Methods
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "universes",
				columns: table => new
				{
					id = table.Column<string>(type: "TEXT", nullable: false),
					is_custom = table.Column<bool>(type: "INTEGER", nullable: false),
					parent_id = table.Column<string>(type: "TEXT", nullable: true),
					name = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_universes", x => x.id);
					table.ForeignKey(
						name: "fk_universes_universes_parent_id",
						column: x => x.parent_id,
						principalTable: "universes",
						principalColumn: "id");
				});

			migrationBuilder.InsertData(
				table: "universes",
				columns: ["id", "is_custom", "name", "parent_id"],
				values: ["real", false, "Real", null]);

			migrationBuilder.CreateIndex(
				name: "ix_universe_unique_name",
				table: "universes",
				column: "name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "ix_universes_parent_id",
				table: "universes",
				column: "parent_id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "universes");
		}
		#endregion
	}
}
