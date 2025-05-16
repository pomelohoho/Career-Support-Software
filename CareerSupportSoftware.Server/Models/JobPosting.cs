// Models/JobPosting.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CareerSupportSoftware.Server.Models;

public class JobPosting
{
    [Key] public int JobPostingId { get; set; }

    // RapidAPI id field used for deduplication
    [Required, MaxLength(50)]
    public string ExternalId { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Organization { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Url { get; set; }

    public DateTime DatePosted { get; set; }
    public DateTime DateFetchedUtc { get; set; } = DateTime.UtcNow;

    // Raw JSON blobs straight from the API 
    public string? LocationsRawJson { get; set; }
    public string? SalaryRawJson { get; set; }

    // Lightly processed data we do want to query on 
    public List<string>? LocationsDerived { get; set; }

    [JsonPropertyName("isSponsor")]
    public bool IsVisaSponsor { get; set; }

}
