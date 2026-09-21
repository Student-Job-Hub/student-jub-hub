using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentJobHub.Api.DTOs.Reviews;
using StudentJobHub.Api.Services;

namespace StudentJobHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly ReviewService _reviewService;

    public ReviewsController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // POST: api/reviews
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateReviewDto dto)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var result = await _reviewService.CreateAsync(
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
            new { id = result.Review!.Id },
            result.Review);
    }

    // GET: api/reviews/user/{userId}
    [HttpGet("user/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserReviews(
        string userId)
    {
        var reviews =
            await _reviewService.GetUserReviewsAsync(userId);

        return Ok(reviews);
    }

    // GET: api/reviews/{id}
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var review = await _reviewService.GetByIdAsync(id);

        if (review == null)
        {
            return NotFound(new
            {
                message = "Review not found."
            });
        }

        return Ok(review);
    }

    // DELETE: api/reviews/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var deleted = await _reviewService.DeleteAsync(
            id,
            userId);

        if (!deleted)
        {
            return NotFound(new
            {
                message =
                    "Review not found or you are not the reviewer."
            });
        }

        return Ok(new
        {
            message = "Review deleted successfully."
        });
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue("sub");
    }
}