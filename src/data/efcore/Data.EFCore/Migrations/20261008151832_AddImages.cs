#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Add the images table.")]
	public partial class AddImages : Migration
	{
		#region Methods
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "images",
				columns: table => new
				{
					id = table.Column<string>(type: "TEXT", nullable: false),
					kind_id = table.Column<string>(type: "TEXT", nullable: false),
					width = table.Column<int>(type: "INTEGER", nullable: false),
					height = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_images", x => x.id);
					table.ForeignKey(
						name: "fk_images_image_kinds_kind_id",
						column: x => x.kind_id,
						principalTable: "image_kinds",
						principalColumn: "id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "ix_images_kind_id",
				table: "images",
				column: "kind_id");

			migrationBuilder.AddForeignKey(
				name: "fk_image_resolution_images_image_id",
				table: "image_resolution",
				column: "image_id",
				principalTable: "images",
				principalColumn: "id",
				onDelete: ReferentialAction.Cascade);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "fk_image_resolution_images_image_id",
				table: "image_resolution");

			migrationBuilder.DropTable(
				name: "images");
		}
		#endregion
	}
}
