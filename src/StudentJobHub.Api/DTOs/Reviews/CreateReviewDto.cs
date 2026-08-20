using System.ComponentModel.DataAnnotations;

namespace StudentJobHub.Api.DTOs.Reviews;

public class CreateReviewDto
{
    [Required]
    public string RevieweeId { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(1000)]
    public string Comment { get; set; } = string.Empty;
}