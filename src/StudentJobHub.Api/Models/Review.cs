namespace StudentJobHub.Api.Models;

public class Review
{
    public int Id { get; set; }

    public string ReviewerId { get; set; } = string.Empty;

    public ApplicationUser? Reviewer { get; set; }

    public string RevieweeId { get; set; } = string.Empty;

    public ApplicationUser? Reviewee { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}