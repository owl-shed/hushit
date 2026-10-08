#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Fix image_resolutions table name.")]
	public partial class FixImageResolutionsTableName : Migration
	{
		#region Methods
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "fk_image_resolution_images_image_id",
				table: "image_resolution");

			migrationBuilder.DropPrimaryKey(
				name: "pk_image_resolution",
				table: "image_resolution");

			migrationBuilder.RenameTable(
				name: "image_resolution",
				newName: "image_resolutions");

			migrationBuilder.AddPrimaryKey(
				name: "pk_image_resolutions",
				table: "image_resolutions",
				column: "id");

			migrationBuilder.AddForeignKey(
				name: "fk_image_resolutions_images_image_id",
				table: "image_resolutions",
				column: "image_id",
				principalTable: "images",
				principalColumn: "id",
				onDelete: ReferentialAction.Cascade);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "fk_image_resolutions_images_image_id",
				table: "image_resolutions");

			migrationBuilder.DropPrimaryKey(
				name: "pk_image_resolutions",
				table: "image_resolutions");

			migrationBuilder.RenameTable(
				name: "image_resolutions",
				newName: "image_resolution");

			migrationBuilder.AddPrimaryKey(
				name: "pk_image_resolution",
				table: "image_resolution",
				column: "id");

			migrationBuilder.AddForeignKey(
				name: "fk_image_resolution_images_image_id",
				table: "image_resolution",
				column: "image_id",
				principalTable: "images",
				principalColumn: "id",
				onDelete: ReferentialAction.Cascade);
		}
		#endregion
	}
}
