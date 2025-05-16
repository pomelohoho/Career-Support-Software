// Services/JobService.cs
using CareerSupportSoftware.Server.Data;
using CareerSupportSoftware.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;        
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CareerSupportSoftware.Server.Services;


/// Pulls jobs from RapidAPI, filters to H-1B sponsors, dedupes, and saves.

public class JobService
{
    private readonly JobDbContext _db;
    private readonly IHttpClientFactory _httpFactory;
    private readonly ILogger<JobService> _log;

    public JobService(JobDbContext db,
                      IHttpClientFactory httpFactory,
                      ILogger<JobService> log)
    {
        _db = db;
        _httpFactory = httpFactory;
        _log = log;
    }

    public async Task<List<JobPosting>> GetAllJobsAsync(
    int limit = 100,
    bool includeNonSponsors = true,
    CancellationToken ct = default)
    {
        // Start with base query
        var query = _db.JobPostings
            .AsNoTracking()
            .AsQueryable(); 

        // Apply sponsor filter
        if (!includeNonSponsors)
        {
            query = query.Where(j => j.IsVisaSponsor);
        }

        // Order and limit
        var result = await query
            .OrderByDescending(j => j.DatePosted)
            .Take(limit)
            .Select(j => new JobPosting
            {
                JobPostingId = j.JobPostingId,
                Title = j.Title,
                Organization = j.Organization,
                Url = j.Url,
                DatePosted = j.DatePosted,
                LocationsDerived = j.LocationsDerived,
                SalaryRawJson = j.SalaryRawJson,
                IsVisaSponsor = j.IsVisaSponsor,
                DateFetchedUtc = j.DateFetchedUtc,
                ExternalId = j.ExternalId
            })
            .ToListAsync(ct);

        return result;
    }
    public async Task<int> FetchAndSaveLatestJobsAsync(CancellationToken ct = default)
    {
        var client = _httpFactory.CreateClient("RapidAPI");

        const string endpoint =
            "active-jb-7d?limit=100&offset=0"
          + "&title_filter=%22Data%20Engineer%22"
          + "&location_filter=%22United%20States%22%20OR%20%22United%20Kingdom%22";

        using var resp = await client.GetAsync(endpoint, ct);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync(ct);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var dtoList = JsonSerializer.Deserialize<List<RapidJobDto>>(json, options) ?? new();

        // sponsor filter 
        var sponsorNames = await _db.H1BCompanies
                                    .Select(h => h.NormalizedName)
                                    .ToListAsync(ct);

        var toInsert = new List<JobPosting>();

        foreach (var dto in dtoList)
        {
            var normOrg = dto.organization.ToUpperInvariant().Trim();
            //if (!sponsorNames.Contains(normOrg)) continue;          // skip non-sponsors
            bool isSponsor = sponsorNames.Contains(normOrg);

            bool exists = await _db.JobPostings
                                   .AnyAsync(j => j.ExternalId == dto.id, ct);
            if (exists) continue;                                   // skip duplicates

            toInsert.Add(new JobPosting
            {
                ExternalId = dto.id,
                Title = dto.title,
                Organization = dto.organization,
                Url = dto.url,
                DatePosted = dto.date_posted,

                LocationsRawJson = dto.locations_raw.Value.GetRawText(),

                SalaryRawJson = dto.salary_raw.HasValue
                                   ? dto.salary_raw.Value.GetRawText()
                                   : null,

                LocationsDerived = dto.locations_derived,
                IsVisaSponsor = isSponsor,      
                DateFetchedUtc = DateTime.UtcNow
            });
        }

        if (toInsert.Count == 0) return 0;

        _db.JobPostings.AddRange(toInsert);
        await _db.SaveChangesAsync(ct);

        _log.LogInformation("Inserted {Count} new jobs", toInsert.Count);
        return toInsert.Count;
    }
}
