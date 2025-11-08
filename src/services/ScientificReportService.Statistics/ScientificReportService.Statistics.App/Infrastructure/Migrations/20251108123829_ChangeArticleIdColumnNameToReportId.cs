using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScientificReportService.Statistics.App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeArticleIdColumnNameToReportId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_report_event_report_article_id",
                table: "report_event");

            migrationBuilder.RenameColumn(
                name: "article_id",
                table: "report_event",
                newName: "report_id");

            migrationBuilder.RenameIndex(
                name: "IX_report_event_article_id",
                table: "report_event",
                newName: "IX_report_event_report_id");

            migrationBuilder.AddForeignKey(
                name: "FK_report_event_report_report_id",
                table: "report_event",
                column: "report_id",
                principalTable: "report",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_report_event_report_report_id",
                table: "report_event");

            migrationBuilder.RenameColumn(
                name: "report_id",
                table: "report_event",
                newName: "article_id");

            migrationBuilder.RenameIndex(
                name: "IX_report_event_report_id",
                table: "report_event",
                newName: "IX_report_event_article_id");

            migrationBuilder.AddForeignKey(
                name: "FK_report_event_report_article_id",
                table: "report_event",
                column: "article_id",
                principalTable: "report",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
