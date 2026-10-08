#nullable disable

namespace OwlShed.Hushit.Data.EFCore.Migrations
{
	/// <inheritdoc />
	[Description("Add the audio fingerprint kinds table.")]
	public partial class AddAudioFingerprintKinds : Migration
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

			migrationBuilder.InsertData(
				table: "audio_fingerprint_kinds",
				column: "id",
				value: "chromaprint");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "audio_fingerprint_kinds");
		}
		#endregion
	}
}
