using System.ComponentModel.DataAnnotations;
namespace StudentJobHub.Client.Models;

public class CreateJobModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; } = "Tech & IT";

    [Required(ErrorMessage = "Requirements are required.")]
    [StringLength(1000, ErrorMessage = "Requirements cannot exceed 1000 characters.")]
    public string Requirements { get; set; } = string.Empty;

    [Range(1, 100000, ErrorMessage = "Budget must be greater than 0.")]
    public decimal Budget { get; set; }

    [Required(ErrorMessage = "Deadline is required.")]
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