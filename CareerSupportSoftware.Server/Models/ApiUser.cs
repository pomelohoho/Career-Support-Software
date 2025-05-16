// Models/ApiUser.cs
using Microsoft.AspNetCore.Identity;

namespace CareerSupportSoftware.Server.Models;

public class ApiUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public virtual ICollection<UserPreference> Preferences { get; set; }

    public DateTime AccountCreated { get; set; } = DateTime.UtcNow;
}