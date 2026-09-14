namespace StudentJobHub.Client.Models;

public class ReviewModel
{
    public int Id { get; set; }

    public string ReviewerId { get; set; } = string.Empty;

    public string ReviewerName { get; set; } = string.Empty;

    public string RevieweeId { get; set; } = string.Empty;

    public string RevieweeName { get; set; } = string.Empty;

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class CreateReviewModel
{
    public string RevieweeId { get; set; } = string.Empty;

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;
}