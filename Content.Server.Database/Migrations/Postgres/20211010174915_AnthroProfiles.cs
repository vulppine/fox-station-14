using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Content.Server.Database.Migrations.Postgres
{
    public partial class AnthroProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anthro_system");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "anthro_system",
                columns: table => new
                {
                    anthro_system_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    markings = table.Column<string>(type: "text", nullable: false),
                    profile_id = table.Column<int>(type: "integer", nullable: false),
                    species_base = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anthro_system", x => x.anthro_system_id);
                    table.ForeignKey(
                        name: "FK_anthro_system_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "profile",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_anthro_system_profile_id",
                table: "anthro_system",
                column: "profile_id",
                unique: true);
        }
    }
}
