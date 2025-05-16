// Models/UserPreference.cs
using CareerSupportSoftware.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerSupportSoftware.Server.Models;
public class UserPreference
{
    [Key]
    public int PreferenceId { get; set; }
    [Required]
    public string UserId { get; set; } = string.Empty;
    [Required]
    public byte[] EncryptedData { get; set; } = Array.Empty<byte>();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserId")]
    public ApiUser? User { get; set; }
}