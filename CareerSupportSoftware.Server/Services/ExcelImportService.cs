// Services/ExcelImportService.cs
using CareerSupportSoftware.Server.Data;
using CareerSupportSoftware.Server.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;
using System.Threading;

namespace CareerSupportSoftwareServer.Services;

public class ExcelImportService
{
    private readonly JobDbContext _context;

    public ExcelImportService(JobDbContext context)
    {
        _context = context;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task ImportH1BData(Stream fileStream, CancellationToken ct = default)
    {
        using var package = new ExcelPackage(fileStream);
        var ws = package.Workbook.Worksheets[0];

        var companies = new List<H1BCompany>();

        CultureInfo enUS = CultureInfo.GetCultureInfo("en-US");

        decimal ParseMoney(string raw)
        {
            var clean = raw.Replace("$", "").Replace(",", "").Trim();
            return decimal.TryParse(clean, NumberStyles.Any, enUS, out var d) ? d : 0m;
        }

        DateTime ParseDate(string raw)
        {
            return DateTime.TryParse(raw, enUS, DateTimeStyles.None, out var dt)
                   ? dt : DateTime.MinValue;         
        }

        for (int row = 2; row <= ws.Dimension.End.Row; row++)
        {
            // skip rows with no CASE_NUMBER
            if (string.IsNullOrWhiteSpace(ws.Cells[row, 1].Text)) continue;

            string employer = ws.Cells[row, 20].Text.Trim();

            companies.Add(new H1BCompany
            {
                CaseNumber = ws.Cells[row, 1].Text,
                EmployerName = employer,
                NormalizedName = employer.ToUpperInvariant(),
                JobTitle = ws.Cells[row, 7].Text,
                WorksiteCity = ws.Cells[row, 62].Text,
                WorksiteState = ws.Cells[row, 64].Text,
                PrevailingWage = ParseMoney(ws.Cells[row, 68].Text),
                CertifiedDate = ParseDate(ws.Cells[row, 5].Text),
                VisaClass = ws.Cells[row, 6].Text,
                IsActive = ws.Cells[row, 2].Text.Equals("Certified",
                                 StringComparison.OrdinalIgnoreCase)
            });
        }
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE dbo.H1BCompanies");

        var bulkConfig = new BulkConfig
        {
            BatchSize = 5000,
            BulkCopyTimeout = 300
        };

        await _context.BulkInsertAsync(companies, bulkConfig, cancellationToken: ct);

    }

}