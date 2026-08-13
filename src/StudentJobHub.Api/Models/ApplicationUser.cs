using Microsoft.AspNetCore.Identity;

namespace StudentJobHub.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public string? ProfilePictureUrl { get; set; }

    public string? Bio { get; set; }

    public string? University { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}