using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentJobHub.Api.Services;

namespace StudentJobHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationsController(
        NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    // GET: api/notifications
    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var notifications =
            await _notificationService.GetUserNotificationsAsync(userId);

        return Ok(notifications);
    }

    // PATCH: api/notifications/{id}/read
    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var success =
            await _notificationService.MarkAsReadAsync(
                id,
                userId);

        if (!success)
        {
            return NotFound(new
            {
                message = "Notification not found."
            });
        }

        return Ok(new
        {
            message = "Notification marked as read."
        });
    }

    // DELETE: api/notifications/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var success =
            await _notificationService.DeleteAsync(
                id,
                userId);

        if (!success)
        {
            return NotFound(new
            {
                message = "Notification not found."
            });
        }

        return Ok(new
        {
            message = "Notification deleted successfully."
        });
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue("sub");
    }
}