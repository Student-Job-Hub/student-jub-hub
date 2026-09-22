using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentJobHub.Api.DTOs.Services;
using StudentJobHub.Api.Services;

namespace StudentJobHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ServiceService _serviceService;

    public ServicesController(ServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    // ==========================================
    // GET ALL SERVICES
    // ==========================================

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var services = await _serviceService.GetAllAsync();

        return Ok(services);
    }

    // ==========================================
    // GET MY SERVICES
    // ==========================================

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMy()
    {
        var providerId = GetCurrentUserId();

        if (providerId == null)
        {
            return Unauthorized();
        }

        var services = await _serviceService.GetByProviderIdAsync(providerId);

        return Ok(services);
    }

    // ==========================================
    // GET SERVICE BY ID
    // ==========================================

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var service = await _serviceService.GetByIdAsync(id);

        if (service == null)
        {
            return NotFound(new
            {
                message = "Service not found."
            });
        }

        return Ok(service);
    }

    // ==========================================
    // CREATE SERVICE
    // ==========================================

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        CreateServiceDto dto)
    {
        var providerId = GetCurrentUserId();

        if (providerId == null)
        {
            return Unauthorized();
        }

        var service = await _serviceService.CreateAsync(
            dto,
            providerId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = service.Id },
            service);
    }

    // ==========================================
    // UPDATE SERVICE
    // ==========================================

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(
        int id,
        UpdateServiceDto dto)
    {
        var providerId = GetCurrentUserId();

        if (providerId == null)
        {
            return Unauthorized();
        }

        var updated = await _serviceService.UpdateAsync(
            id,
            dto,
            providerId);

        if (!updated)
        {
            return NotFound(new
            {
                message =
                    "Service not found or you do not own this service."
            });
        }

        return Ok(new
        {
            message = "Service updated successfully."
        });
    }

    // ==========================================
    // DELETE SERVICE
    // ==========================================

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var providerId = GetCurrentUserId();

        if (providerId == null)
        {
            return Unauthorized();
        }

        var deleted = await _serviceService.DeleteAsync(
            id,
            providerId);

        if (!deleted)
        {
            return NotFound(new
            {
                message =
                    "Service not found or you do not own this service."
            });
        }

        return Ok(new
        {
            message = "Service deleted successfully."
        });
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(
            ClaimTypes.NameIdentifier);
    }
}