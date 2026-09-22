using System.ComponentModel.DataAnnotations;

namespace StudentJobHub.Api.DTOs.Jobs;

public class CreateJobDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = "General";

    [Required]
    [StringLength(1000)]
    public string Requirements { get; set; } = string.Empty;

    [Range(0.01, 1000000)]
    public decimal Budget { get; set; }

    [Required]
    public DateTime Deadline { get; set; }
}