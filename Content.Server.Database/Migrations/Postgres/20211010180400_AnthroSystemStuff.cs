using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Content.Server.Database.Migrations.Postgres
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
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "species_base",
                table: "profile",
                type: "text",
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
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "anthro_profile",
                columns: table => new
                {
                    anthro_profile_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    markings = table.Column<string>(type: "text", nullable: false),
                    species_base = table.Column<string>(type: "text", nullable: false)
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
