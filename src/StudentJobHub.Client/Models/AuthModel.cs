namespace StudentJobHub.Client.Models;

public class LoginModel
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class RegisterModel
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "Student";

    public string? University { get; set; }
}

public class AuthResponse
{
    public string Message { get; set; } = string.Empty;

    public string? Token { get; set; }
}