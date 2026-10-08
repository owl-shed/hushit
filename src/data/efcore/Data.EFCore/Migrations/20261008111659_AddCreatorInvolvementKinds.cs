#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Add the creator involvement kinds table.")]
	public partial class AddCreatorInvolvementKinds : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "creator_involvement_kinds",
				columns: table => new
				{
					id = table.Column<string>(type: "TEXT", nullable: false),
					is_custom = table.Column<bool>(type: "INTEGER", nullable: false),
					name = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_creator_involvement_kinds", x => x.id);
				});

			migrationBuilder.InsertData(
				table: "creator_involvement_kinds",
				columns: ["id", "is_custom", "name"],
				values: new object[,]
				{
					{ "artist", false, "Artist" },
					{ "composer", false, "Composer" },
					{ "lyricist", false, "Lyricist" }
				});

			migrationBuilder.CreateIndex(
				name: "ix_creator_involvement_kind_unique_name",
				table: "creator_involvement_kinds",
				column: "name",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "creator_involvement_kinds");
		}
	}
}
