using Microsoft.EntityFrameworkCore.Migrations;

namespace Content.Server.Database.Migrations.Sqlite
{
    public partial class AnthroSystemStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_profile_anthro_profile_anthro_profile_id",
                table: "profile");

            migrationBuilder.DropTable(
                name: "anthro_profile");

            migrationBuilder.DropIndex(
                name: "IX_profile_anthro_profile_id",
                table: "profile");

            migrationBuilder.DropColumn(
                name: "anthro_profile_id",
                table: "profile");

            migrationBuilder.AddColumn<string>(
                name: "markings",
                table: "profile",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "species_base",
                table: "profile",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "markings",
                table: "profile");

            migrationBuilder.DropColumn(
                name: "species_base",
                table: "profile");

            migrationBuilder.AddColumn<int>(
                name: "anthro_profile_id",
                table: "profile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "anthro_profile",
                columns: table => new
                {
                    anthro_profile_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    markings = table.Column<string>(type: "TEXT", nullable: false),
                    species_base = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anthro_profile", x => x.anthro_profile_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_profile_anthro_profile_id",
                table: "profile",
                column: "anthro_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_anthro_profile_anthro_profile_id",
                table: "anthro_profile",
                column: "anthro_profile_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_profile_anthro_profile_anthro_profile_id",
                table: "profile",
                column: "anthro_profile_id",
                principalTable: "anthro_profile",
                principalColumn: "anthro_profile_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
