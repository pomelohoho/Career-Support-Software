// Models/H1BCompany.cs
using System.ComponentModel.DataAnnotations;

namespace CareerSupportSoftware.Server.Models;
public class H1BCompany
{
    [Key]
    public int CaseId { get; set; }

    [Required]
    public string CaseNumber { get; set; } = string.Empty;

    [Required]
    public string EmployerName { get; set; } = string.Empty;

    [Required]
    public string NormalizedName { get; set; } = string.Empty; // For case-insensitive matching

    public string JobTitle { get; set; } = string.Empty;
    public string WorksiteCity { get; set; } = string.Empty;
    public string WorksiteState { get; set; } = string.Empty;
    public decimal PrevailingWage { get; set; }
    public DateTime CertifiedDate { get; set; }

    [Required]
    public string VisaClass { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}