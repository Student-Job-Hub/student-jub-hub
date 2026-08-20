using Microsoft.EntityFrameworkCore;
using StudentJobHub.Api.Data;
using StudentJobHub.Api.DTOs.Reviews;
using StudentJobHub.Api.Models;

namespace StudentJobHub.Api.Services;

public class ReviewService
{
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string Message, ReviewResponseDto? Review)>
        CreateAsync(
            CreateReviewDto dto,
            string reviewerId)
    {
        if (reviewerId == dto.RevieweeId)
        {
            return (false, "You cannot review yourself.", null);
        }

        var reviewee = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == dto.RevieweeId);

        if (reviewee == null)
        {
            return (false, "User being reviewed was not found.", null);
        }

        var existingReview = await _context.Reviews
            .AnyAsync(r =>
                r.ReviewerId == reviewerId &&
                r.RevieweeId == dto.RevieweeId);

        if (existingReview)
        {
            return (false, "You have already reviewed this user.", null);
        }

        var review = new Review
        {
            ReviewerId = reviewerId,
            RevieweeId = dto.RevieweeId,
            Rating = dto.Rating,
            Comment = dto.Comment
        };

        _context.Reviews.Add(review);

        await _context.SaveChangesAsync();

        return (
            true,
            "Review created successfully.",
            await GetByIdAsync(review.Id));
    }

    public async Task<List<ReviewResponseDto>> GetUserReviewsAsync(
        string userId)
    {
        return await _context.Reviews
            .Include(r => r.Reviewer)
            .Include(r => r.Reviewee)
            .Where(r => r.RevieweeId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReviewResponseDto
            {
                Id = r.Id,

                ReviewerId = r.ReviewerId,

                ReviewerName = r.Reviewer != null
                    ? r.Reviewer.FullName
                    : string.Empty,

                RevieweeId = r.RevieweeId,

                RevieweeName = r.Reviewee != null
                    ? r.Reviewee.FullName
                    : string.Empty,

                Rating = r.Rating,

                Comment = r.Comment,

                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ReviewResponseDto?> GetByIdAsync(int id)
    {
        return await _context.Reviews
            .Include(r => r.Reviewer)
            .Include(r => r.Reviewee)
            .Where(r => r.Id == id)
            .Select(r => new ReviewResponseDto
            {
                Id = r.Id,

                ReviewerId = r.ReviewerId,

                ReviewerName = r.Reviewer != null
                    ? r.Reviewer.FullName
                    : string.Empty,

                RevieweeId = r.RevieweeId,

                RevieweeName = r.Reviewee != null
                    ? r.Reviewee.FullName
                    : string.Empty,

                Rating = r.Rating,

                Comment = r.Comment,

                CreatedAt = r.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteAsync(
        int id,
        string reviewerId)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r =>
                r.Id == id &&
                r.ReviewerId == reviewerId);

        if (review == null)
        {
            return false;
        }

        _context.Reviews.Remove(review);

        await _context.SaveChangesAsync();

        return true;
    }
}