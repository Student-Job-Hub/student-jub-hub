using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentJobHub.Api.DTOs.Jobs;
using StudentJobHub.Api.Services;

namespace StudentJobHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;

    public JobsController(JobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var jobs = await _jobService.GetAllAsync();

        return Ok(jobs);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyJobs()
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var jobs = await _jobService.GetMyJobsAsync(userId);

        return Ok(jobs);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await _jobService.GetByIdAsync(id);

        if (job == null)
        {
            return NotFound(new
            {
                message = "Job not found."
            });
        }

        return Ok(job);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        CreateJobDto dto)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var job = await _jobService.CreateAsync(
            dto,
            userId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = job.Id },
            job);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(
        int id,
        UpdateJobDto dto)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var updated = await _jobService.UpdateAsync(
            id,
            dto,
            userId);

        if (!updated)
        {
            return NotFound(new
            {
                message =
                    "Job not found or you do not own this job."
            });
        }

        return Ok(new
        {
            message = "Job updated successfully."
        });
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var deleted = await _jobService.DeleteAsync(
            id,
            userId);

        if (!deleted)
        {
            return NotFound(new
            {
                message =
                    "Job not found or you do not own this job."
            });
        }

        return Ok(new
        {
            message = "Job deleted successfully."
        });
    }

    [HttpPatch("{id:int}/close")]
    [Authorize]
    public async Task<IActionResult> Close(int id)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var closed = await _jobService.CloseAsync(
            id,
            userId);

        if (!closed)
        {
            return NotFound(new
            {
                message =
                    "Job not found or you do not own this job."
            });
        }

        return Ok(new
        {
            message = "Job closed successfully."
        });
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(
            ClaimTypes.NameIdentifier);
    }
}