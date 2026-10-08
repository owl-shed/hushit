#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Switch image resolutions to have a composite key.")]
	public partial class SwitchImageResolutionsToHaveACompositeKey : Migration
	{
		#region Methods
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropPrimaryKey(
				name: "pk_image_resolutions",
				table: "image_resolutions");

			migrationBuilder.DropIndex(
				name: "ix_image_resolution_unique_resolution",
				table: "image_resolutions");

			migrationBuilder.DropColumn(
				name: "id",
				table: "image_resolutions");

			migrationBuilder.AlterColumn<int>(
				name: "width",
				table: "image_resolutions",
				type: "INTEGER",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "INTEGER")
				.Annotation("Relational:ColumnOrder", 1)
				.OldAnnotation("Relational:ColumnOrder", 2);

			migrationBuilder.AlterColumn<string>(
				name: "image_id",
				table: "image_resolutions",
				type: "TEXT",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "TEXT")
				.Annotation("Relational:ColumnOrder", 0)
				.OldAnnotation("Relational:ColumnOrder", 1);

			migrationBuilder.AddPrimaryKey(
				name: "pk_image_resolutions",
				table: "image_resolutions",
				columns: ["image_id", "width", "height"]);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropPrimaryKey(
				name: "pk_image_resolutions",
				table: "image_resolutions");

			migrationBuilder.AlterColumn<int>(
				name: "width",
				table: "image_resolutions",
				type: "INTEGER",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "INTEGER")
				.Annotation("Relational:ColumnOrder", 2)
				.OldAnnotation("Relational:ColumnOrder", 1);

			migrationBuilder.AlterColumn<string>(
				name: "image_id",
				table: "image_resolutions",
				type: "TEXT",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "TEXT")
				.Annotation("Relational:ColumnOrder", 1)
				.OldAnnotation("Relational:ColumnOrder", 0);

			migrationBuilder.AddColumn<int>(
				name: "id",
				table: "image_resolutions",
				type: "INTEGER",
				nullable: false,
				defaultValue: 0)
				.Annotation("Relational:ColumnOrder", 0)
				.Annotation("Sqlite:Autoincrement", true);

			migrationBuilder.AddPrimaryKey(
				name: "pk_image_resolutions",
				table: "image_resolutions",
				column: "id");

			migrationBuilder.CreateIndex(
				name: "ix_image_resolution_unique_resolution",
				table: "image_resolutions",
				columns: ["image_id", "width", "height"],
				unique: true);
		}
		#endregion
	}
}
