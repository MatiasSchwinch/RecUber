using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecUber.Migrations
{
    public partial class CreateDatabaseSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Egress",
                columns: table => new
                {
                    EgressId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false),
                    TotalValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egress", x => x.EgressId);
                });

            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<double>(type: "REAL", nullable: false),
                    Distance = table.Column<float>(type: "REAL", nullable: false),
                    Fee = table.Column<decimal>(type: "TEXT", nullable: false),
                    Profit = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.EntryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Egress");

            migrationBuilder.DropTable(
                name: "Entry");
        }
    }
}
