#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Fix some delete cascades.")]
	public partial class FixSomeDeleteCascades : Migration
	{
		#region Methods
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "fk_images_image_kinds_kind_id",
				table: "images");

			migrationBuilder.DropForeignKey(
				name: "fk_universes_universes_parent_id",
				table: "universes");

			migrationBuilder.AddForeignKey(
				name: "fk_images_image_kinds_kind_id",
				table: "images",
				column: "kind_id",
				principalTable: "image_kinds",
				principalColumn: "id");

			migrationBuilder.AddForeignKey(
				name: "fk_universes_universes_parent_id",
				table: "universes",
				column: "parent_id",
				principalTable: "universes",
				principalColumn: "id",
				onDelete: ReferentialAction.Cascade);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "fk_images_image_kinds_kind_id",
				table: "images");

			migrationBuilder.DropForeignKey(
				name: "fk_universes_universes_parent_id",
				table: "universes");

			migrationBuilder.AddForeignKey(
				name: "fk_images_image_kinds_kind_id",
				table: "images",
				column: "kind_id",
				principalTable: "image_kinds",
				principalColumn: "id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "fk_universes_universes_parent_id",
				table: "universes",
				column: "parent_id",
				principalTable: "universes",
				principalColumn: "id");
		}
		#endregion
	}
}
