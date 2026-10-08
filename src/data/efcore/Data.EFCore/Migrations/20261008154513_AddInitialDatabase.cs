#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Add the initial database tables.")]
	public partial class AddInitialDatabase : Migration
	{
		#region Methods
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "audio_fingerprint_kinds",
				columns: table => new
				{
					id = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_audio_fingerprint_kinds", x => x.id);
				});

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
						principalColumn: "id",
						onDelete: ReferentialAction.Cascade);
				});

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
						principalColumn: "id");
				});

			migrationBuilder.CreateTable(
				name: "image_resolutions",
				columns: table => new
				{
					image_id = table.Column<string>(type: "TEXT", nullable: false),
					width = table.Column<int>(type: "INTEGER", nullable: false),
					height = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_image_resolutions", x => new { x.image_id, x.width, x.height });
					table.ForeignKey(
						name: "fk_image_resolutions_images_image_id",
						column: x => x.image_id,
						principalTable: "images",
						principalColumn: "id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.InsertData(
				table: "audio_fingerprint_kinds",
				column: "id",
				value: "chromaprint");

			migrationBuilder.InsertData(
				table: "creator_involvement_kinds",
				columns: new[] { "id", "is_custom", "name" },
				values: new object[,]
				{
					{ "artist", false, "Artist" },
					{ "composer", false, "Composer" },
					{ "lyricist", false, "Lyricist" }
				});

			migrationBuilder.InsertData(
				table: "hash_kinds",
				column: "id",
				value: "sha256");

			migrationBuilder.InsertData(
				table: "image_kinds",
				columns: new[] { "id", "unknown_kind_description" },
				values: new object[,]
				{
					{ "back_cover", null },
					{ "front_cover", null }
				});

			migrationBuilder.InsertData(
				table: "universes",
				columns: new[] { "id", "is_custom", "name", "parent_id" },
				values: new object[] { "real", false, "Real", null });

			migrationBuilder.CreateIndex(
				name: "ix_creator_involvement_kind_unique_name",
				table: "creator_involvement_kinds",
				column: "name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "ix_image_kind_unknown_kind_description_unique_name",
				table: "image_kinds",
				column: "unknown_kind_description",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "ix_images_kind_id",
				table: "images",
				column: "kind_id");

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
				name: "audio_fingerprint_kinds");

			migrationBuilder.DropTable(
				name: "creator_involvement_kinds");

			migrationBuilder.DropTable(
				name: "hash_kinds");

			migrationBuilder.DropTable(
				name: "image_resolutions");

			migrationBuilder.DropTable(
				name: "universes");

			migrationBuilder.DropTable(
				name: "images");

			migrationBuilder.DropTable(
				name: "image_kinds");
		}
		#endregion
	}
}
