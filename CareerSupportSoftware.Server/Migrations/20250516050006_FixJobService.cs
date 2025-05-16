using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSupportSoftware.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixJobService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_ExternalId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "PostedDate",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "SalaryMax",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "SalaryMin",
                table: "JobPostings");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "JobPostings",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "JobUrl",
                table: "JobPostings",
                newName: "SalaryRaw");

            migrationBuilder.RenameColumn(
                name: "Company",
                table: "JobPostings",
                newName: "SourceType");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "JobPostings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CitiesDerived",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountriesDerived",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "JobPostings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePosted",
                table: "JobPostings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateValidThrough",
                table: "JobPostings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "LocationsDerived",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationsRaw",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization",
                table: "JobPostings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationLogo",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationUrl",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionsDerived",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RemoteDerived",
                table: "JobPostings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceDomain",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_DatePosted",
                table: "JobPostings",
                column: "DatePosted");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_Organization",
                table: "JobPostings",
                column: "Organization");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_DatePosted",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_Organization",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "CitiesDerived",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "CountriesDerived",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "DatePosted",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "DateValidThrough",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "LocationsDerived",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "LocationsRaw",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "Organization",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "OrganizationLogo",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "OrganizationUrl",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "RegionsDerived",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "RemoteDerived",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "SourceDomain",
                table: "JobPostings");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "JobPostings",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "SourceType",
                table: "JobPostings",
                newName: "Company");

            migrationBuilder.RenameColumn(
                name: "SalaryRaw",
                table: "JobPostings",
                newName: "JobUrl");

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "JobPostings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "JobPostings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostedDate",
                table: "JobPostings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryMax",
                table: "JobPostings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryMin",
                table: "JobPostings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_ExternalId",
                table: "JobPostings",
                column: "ExternalId",
                unique: true,
                filter: "[ExternalId] IS NOT NULL");
        }
    }
}
