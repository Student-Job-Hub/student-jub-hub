namespace StudentJobHub.Api.Models;

public class Service
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string ProviderId { get; set; } = string.Empty;

    public ApplicationUser? Provider { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}