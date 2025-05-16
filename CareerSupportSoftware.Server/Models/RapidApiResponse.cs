using System.Text.Json;

namespace CareerSupportSoftware.Server.Models;

public record RapidJobDto(
    string id,
    string title,
    string organization,
    string url,
    DateTime date_posted,
    List<string>? locations_derived,
    JsonElement? locations_raw,   
    JsonElement? salary_raw       
);
