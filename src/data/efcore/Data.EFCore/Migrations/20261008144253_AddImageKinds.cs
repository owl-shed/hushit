#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Add the image kinds table.")]
	public partial class AddImageKinds : Migration
	{
		#region Methods
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "image_kinds",
				columns: table => new
				{
					id = table.Column<string>(type: "TEXT", nullable: false),
					unknown_kind_description = table.Column<string>(type: "TEXT", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_image_kinds", x => x.id);
				});

			migrationBuilder.InsertData(
				table: "image_kinds",
				columns: ["id", "unknown_kind_description"],
				values: new object[,]
				{
					{ "back_cover", null },
					{ "front_cover", null }
				});

			migrationBuilder.CreateIndex(
				name: "image_kind_unknown_kind_description_unique_name",
				table: "image_kinds",
				column: "unknown_kind_description",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "image_kinds");
		}
		#endregion
	}
}
