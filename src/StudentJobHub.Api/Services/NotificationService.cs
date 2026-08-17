using Microsoft.EntityFrameworkCore;
using StudentJobHub.Api.Data;
using StudentJobHub.Api.DTOs.Notifications;
using StudentJobHub.Api.Models;

namespace StudentJobHub.Api.Services;

public class NotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Notification> CreateAsync(
        string userId,
        string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            IsRead = false
        };

        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync();

        return notification;
    }

    public async Task<List<NotificationResponseDto>> GetUserNotificationsAsync(
        string userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationResponseDto
            {
                Id = n.Id,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> MarkAsReadAsync(
        int notificationId,
        string userId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n =>
                n.Id == notificationId &&
                n.UserId == userId);

        if (notification == null)
        {
            return false;
        }

        notification.IsRead = true;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(
        int notificationId,
        string userId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n =>
                n.Id == notificationId &&
                n.UserId == userId);

        if (notification == null)
        {
            return false;
        }

        _context.Notifications.Remove(notification);

        await _context.SaveChangesAsync();

        return true;
    }
}