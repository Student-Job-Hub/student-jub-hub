using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentJobHub.Api.DTOs.Auth;
using StudentJobHub.Api.Models;

namespace StudentJobHub.Api.Services;

public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<(bool Success, string Message, string? Token)> RegisterAsync(
        RegisterDto dto)
    {
        var allowedRoles = new[] { "Student", "Lecturer", "Business" };

        if (!allowedRoles.Contains(dto.Role, StringComparer.OrdinalIgnoreCase))
        {
            return (false, "Invalid role selected.", null);
        }

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser != null)
        {
            return (false, "A user with this email already exists.", null);
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            University = dto.University
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(error => error.Description));

            return (false, errors, null);
        }

        await _userManager.AddToRoleAsync(
            user,
            NormalizeRole(dto.Role));

        var token = await GenerateJwtTokenAsync(user);

        return (true, "Registration successful.", token);
    }

    public async Task<(bool Success, string Message, string? Token)> LoginAsync(
        LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return (false, "Invalid email or password.", null);
        }

        var validPassword = await _userManager.CheckPasswordAsync(
            user,
            dto.Password);

        if (!validPassword)
        {
            return (false, "Invalid email or password.", null);
        }

        var token = await GenerateJwtTokenAsync(user);

        return (true, "Login successful.", token);
    }

    private async Task<string> GenerateJwtTokenAsync(
        ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        };

        claims.AddRange(
            roles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "JWT key is not configured.");

        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "JWT issuer is not configured.");

        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "JWT audience is not configured.");

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string NormalizeRole(string role)
    {
        return role.ToLowerInvariant() switch
        {
            "student" => "Student",
            "lecturer" => "Lecturer",
            "business" => "Business",
            _ => role
        };
    }
}