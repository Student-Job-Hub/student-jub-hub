using System.ComponentModel.DataAnnotations;

namespace StudentJobHub.Api.DTOs.Applications;

public class CreateApplicationDto
{
    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;
}