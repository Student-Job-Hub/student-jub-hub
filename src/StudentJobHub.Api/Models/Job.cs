namespace StudentJobHub.Api.Models;

public class Job
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Requirements { get; set; } = string.Empty;

    public decimal Budget { get; set; }

    public DateTime Deadline { get; set; }

    public string PostedById { get; set; } = string.Empty;

    public ApplicationUser? PostedBy { get; set; }

    public bool IsOpen { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}