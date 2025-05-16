// Data/JobDbContext.cs
using CareerSupportSoftware.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CareerSupportSoftware.Server.Data;

public class JobDbContext : IdentityDbContext<ApiUser>
{
    public JobDbContext(DbContextOptions<JobDbContext> options)
        : base(options) { }

    public DbSet<JobPosting> JobPostings { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<H1BCompany> H1BCompanies { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<JobPosting>(e =>
        {
            e.HasIndex(j => j.ExternalId).IsUnique();

            // JSON list <-> nvarchar(max) for LocationsDerived
            e.Property(j => j.LocationsDerived)
             .HasConversion(
                 v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                 v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()));

            e.HasIndex(j => j.DatePosted);
            e.HasIndex(j => j.Organization);
            e.HasIndex(j => j.IsVisaSponsor);   
        });

        // H1BCompany Configuration
        builder.Entity<H1BCompany>(entity =>
        {
            entity.HasKey(h => h.CaseId);
            entity.HasIndex(h => h.NormalizedName)
                  .IsClustered(false)
                  .HasDatabaseName("IX_H1BCompanies_NormalizedName");

            entity.Property(h => h.NormalizedName)
                  .IsRequired()
                  .HasMaxLength(255);
        });

        // UserPreference Configuration
        builder.Entity<UserPreference>(entity =>
        {
            entity.HasKey(u => u.PreferenceId);
            entity.HasOne(u => u.User)
                  .WithMany(u => u.Preferences)
                  .HasForeignKey(u => u.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(u => u.EncryptedData)
                  .IsRequired()
                  .HasColumnType("varbinary(max)");
        });

        // ApiUser Configuration
        builder.Entity<ApiUser>(entity =>
        {
            entity.Property(u => u.RefreshToken)
                  .HasMaxLength(500);

            entity.Property(u => u.RefreshTokenExpiry)
                  .HasColumnType("datetime2");
        });
    }
}