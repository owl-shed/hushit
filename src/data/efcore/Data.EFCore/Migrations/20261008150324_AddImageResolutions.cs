#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Add the image resolutions table.")]
	public partial class AddImageResolutions : Migration
	{
		#region Methods
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameIndex(
				name: "image_kind_unknown_kind_description_unique_name",
				table: "image_kinds",
				newName: "ix_image_kind_unknown_kind_description_unique_name");

			migrationBuilder.CreateTable(
				name: "image_resolution",
				columns: table => new
				{
					id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					image_id = table.Column<string>(type: "TEXT", nullable: false),
					width = table.Column<int>(type: "INTEGER", nullable: false),
					height = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_image_resolution", x => x.id);
				});

			migrationBuilder.CreateIndex(
				name: "ix_image_resolution_unique_resolution",
				table: "image_resolution",
				columns: ["image_id", "width", "height"],
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "image_resolution");

			migrationBuilder.RenameIndex(
				name: "ix_image_kind_unknown_kind_description_unique_name",
				table: "image_kinds",
				newName: "image_kind_unknown_kind_description_unique_name");
		}
		#endregion
	}
}
