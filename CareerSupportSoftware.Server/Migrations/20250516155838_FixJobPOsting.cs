using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSupportSoftware.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixJobPOsting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPostings",
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
                name: "DateValidThrough",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "LocationsRaw",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "OffersSponsorship",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "OrganizationLogo",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "OrganizationUrl",
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

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "JobPostings");

            migrationBuilder.RenameColumn(
                name: "SalaryRaw",
                table: "JobPostings",
                newName: "SalaryRawJson");

            migrationBuilder.RenameColumn(
                name: "RegionsDerived",
                table: "JobPostings",
                newName: "LocationsRawJson");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "JobPostings",
                newName: "DateFetchedUtc");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "JobPostings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "JobPostings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Organization",
                table: "JobPostings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "JobPostingId",
                table: "JobPostings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "JobPostings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings",
                column: "JobPostingId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_ExternalId",
                table: "JobPostings",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_ExternalId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "JobPostingId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "JobPostings");

            migrationBuilder.RenameColumn(
                name: "SalaryRawJson",
                table: "JobPostings",
                newName: "SalaryRaw");

            migrationBuilder.RenameColumn(
                name: "LocationsRawJson",
                table: "JobPostings",
                newName: "RegionsDerived");

            migrationBuilder.RenameColumn(
                name: "DateFetchedUtc",
                table: "JobPostings",
                newName: "DateCreated");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Organization",
                table: "JobPostings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

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
                name: "DateValidThrough",
                table: "JobPostings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationsRaw",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OffersSponsorship",
                table: "JobPostings",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings",
                column: "Id");
        }
    }
}
