namespace StudentJobHub.Client.Models;

public class UserModel
{
    public string Id { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; } = string.Empty;

    public string? ProfilePictureUrl { get; set; }

    public string? Bio { get; set; }

    public string? University { get; set; }

    public List<string> Roles { get; set; } = new();
}

public class UpdateProfileModel
{
    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? University { get; set; }

    public string? Bio { get; set; }

    public string? ProfilePictureUrl { get; set; }
}