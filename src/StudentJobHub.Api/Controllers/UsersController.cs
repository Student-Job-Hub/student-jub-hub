using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentJobHub.Api.DTOs.User;
using StudentJobHub.Api.Models;

namespace StudentJobHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Email,
            user.University,
            user.Bio,
            user.ProfilePictureUrl,
            Roles = roles
        });
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateProfileDto dto)
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        if (!string.IsNullOrWhiteSpace(dto.FullName))
        {
            user.FullName = dto.FullName.Trim();
        }

        if (dto.University != null)
        {
            user.University = dto.University.Trim();
        }

        if (dto.Bio != null)
        {
            user.Bio = dto.Bio.Trim();
        }

        if (dto.ProfilePictureUrl != null)
        {
            user.ProfilePictureUrl = dto.ProfilePictureUrl.Trim();
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                message = string.Join("; ", result.Errors.Select(e => e.Description))
            });
        }

        return Ok(new
        {
            message = "Profile updated successfully.",
            user.Id,
            user.FullName,
            user.Email,
            user.University,
            user.Bio,
            user.ProfilePictureUrl
        });
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue("sub");
    }
}

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserLegacy()
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Email,
            user.University,
            user.Bio,
            user.ProfilePictureUrl,
            Roles = roles
        });
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUserLegacy([FromBody] UpdateProfileDto dto)
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        if (!string.IsNullOrWhiteSpace(dto.FullName))
        {
            user.FullName = dto.FullName.Trim();
        }

        if (dto.University != null)
        {
            user.University = dto.University.Trim();
        }

        if (dto.Bio != null)
        {
            user.Bio = dto.Bio.Trim();
        }

        if (dto.ProfilePictureUrl != null)
        {
            user.ProfilePictureUrl = dto.ProfilePictureUrl.Trim();
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                message = string.Join("; ", result.Errors.Select(e => e.Description))
            });
        }

        return Ok(new
        {
            message = "Profile updated successfully.",
            user.Id,
            user.FullName,
            user.Email,
            user.University,
            user.Bio,
            user.ProfilePictureUrl
        });
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue("sub");
    }
}