using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSupportSoftware.Server.Migrations
{
    /// <inheritdoc />
    public partial class H1BAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisaSponsor",
                table: "JobPostings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_IsVisaSponsor",
                table: "JobPostings",
                column: "IsVisaSponsor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobPostings_IsVisaSponsor",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "IsVisaSponsor",
                table: "JobPostings");
        }
    }
}
