using System.ComponentModel.DataAnnotations;

namespace StudentJobHub.Api.DTOs.Applications;

public class UpdateApplicationStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}