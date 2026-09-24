namespace StudentJobHub.Client.Models;

public class CreateNotificationRequest
{
    public string RecipientId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int? ServiceId { get; set; }
}