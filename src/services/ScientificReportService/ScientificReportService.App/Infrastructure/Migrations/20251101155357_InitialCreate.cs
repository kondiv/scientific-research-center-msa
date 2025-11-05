using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScientificReportService.App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scientific_report",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    object_key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    author = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    tags = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scientific_report", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_scientific_report_object_key",
                table: "scientific_report",
                column: "object_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scientific_report_title",
                table: "scientific_report",
                column: "title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scientific_report");
        }
    }
}
