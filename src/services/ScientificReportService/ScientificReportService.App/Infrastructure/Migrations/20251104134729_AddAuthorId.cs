using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScientificReportService.App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "author_id",
                table: "scientific_report",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "author_id",
                table: "scientific_report");
        }
    }
}
