namespace StudentJobHub.Client.Models;

public class CreateJobModel
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Requirements { get; set; } = string.Empty;

    public decimal Budget { get; set; }

    public DateTime Deadline { get; set; } = DateTime.Now.AddDays(7);
}

public class CreateApplicationModel
{
    public string Message { get; set; } = string.Empty;
}

public class UpdateApplicationStatusModel
{
    public string Status { get; set; } = string.Empty;
}