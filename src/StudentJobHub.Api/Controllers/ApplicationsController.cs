using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentJobHub.Api.DTOs.Applications;
using StudentJobHub.Api.Services;

namespace StudentJobHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly JobApplicationService _applicationService;

    public ApplicationsController(
        JobApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    // ==========================================
    // APPLY TO A JOB
    // ==========================================

    [HttpPost("{jobId:int}")]
    public async Task<IActionResult> Create(
        int jobId,
        CreateApplicationDto dto)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var result = await _applicationService.CreateAsync(
            jobId,
            dto,
            userId);

        if (!result.Success)
        {
            return BadRequest(new
            {
                message = result.Message
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Application!.Id },
            result.Application);
    }

    // ==========================================
    // GET MY APPLICATIONS
    // ==========================================

    [HttpGet("my")]
    public async Task<IActionResult> GetMyApplications()
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var applications =
            await _applicationService.GetMyApplicationsAsync(userId);

        return Ok(applications);
    }

    // ==========================================
    // GET APPLICATIONS FOR A JOB
    // ==========================================

    [HttpGet("job/{jobId:int}")]
    public async Task<IActionResult> GetJobApplications(
        int jobId)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var result =
            await _applicationService.GetJobApplicationsAsync(
                jobId,
                userId);

        if (!result.Success)
        {
            return result.Message == "Job not found."
                ? NotFound(new { message = result.Message })
                : Forbid();
        }

        return Ok(result.Applications);
    }

    // ==========================================
    // GET APPLICATION BY ID
    // ==========================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var application =
            await _applicationService.GetByIdAsync(id);

        if (application == null)
        {
            return NotFound(new
            {
                message = "Application not found."
            });
        }

        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        // Only applicant or job owner can view it.
        var job = await _applicationService
            .GetJobApplicationsAsync(
                application.JobId,
                userId);

        var isJobOwner = job.Success;

        if (application.ApplicantId != userId &&
            !isJobOwner)
        {
            return Forbid();
        }

        return Ok(application);
    }

    // ==========================================
    // UPDATE APPLICATION STATUS
    // ==========================================

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateApplicationStatusDto dto)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var result =
            await _applicationService.UpdateStatusAsync(
                id,
                dto.Status,
                userId);

        if (!result.Success)
        {
            if (result.Message == "Application not found.")
            {
                return NotFound(new
                {
                    message = result.Message
                });
            }

            if (result.Message.StartsWith("Invalid status"))
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Forbid();
        }

        return Ok(new
        {
            message = result.Message
        });
    }

    // ==========================================
    // DELETE APPLICATION
    // ==========================================

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var result =
            await _applicationService.DeleteAsync(
                id,
                userId);

        if (!result.Success)
        {
            return BadRequest(new
            {
                message = result.Message
            });
        }

        return Ok(new
        {
            message = result.Message
        });
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(
            ClaimTypes.NameIdentifier);
    }
}