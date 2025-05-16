// Controllers/JobsController.cs
using CareerSupportSoftware.Server.Data;
using CareerSupportSoftware.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerSupportSoftware.Server.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;
    private readonly JobDbContext _db; 

    public JobsController(JobService jobService, JobDbContext db)
    {
        _jobService = jobService;
        _db = db; 
    }

    [HttpPost("ingest")]
    public async Task<IActionResult> Ingest(CancellationToken ct)
    {
        var added = await _jobService.FetchAndSaveLatestJobsAsync(ct);
        return Ok(new { added });
    }

    [HttpGet("test")]
    public async Task<IActionResult> TestEndpoint()
    {
        try
        {
            var testData = new
            {
                DatabaseStatus = _db.Database.CanConnect(),
                JobCount = await _db.JobPostings.AsQueryable().CountAsync(),
                SampleJob = await _db.JobPostings.FirstOrDefaultAsync()
            };

            return Ok(testData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.ToString() });
        }
    }

    [HttpGet]
    [Produces("application/json")] 
    public async Task<IActionResult> GetJobs(
        [FromQuery] int limit = 100,
        [FromQuery] bool includeNonSponsors = true,
        CancellationToken ct = default)
    {
        try
        {
            var jobs = await _jobService.GetAllJobsAsync(limit, includeNonSponsors, ct);
            return Ok(jobs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
