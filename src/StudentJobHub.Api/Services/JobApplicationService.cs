using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StudentJobHub.Api.Data;
using StudentJobHub.Api.DTOs.Applications;
using StudentJobHub.Api.Hubs;
using StudentJobHub.Api.Models;

namespace StudentJobHub.Api.Services;

public class JobApplicationService
{
    private readonly ApplicationDbContext _context;
    private readonly NotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public JobApplicationService(
        ApplicationDbContext context,
        NotificationService notificationService,
        IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _notificationService = notificationService;
        _hubContext = hubContext;
    }

    // =========================================================
    // CREATE APPLICATION
    // =========================================================

    public async Task<(bool Success, string Message, ApplicationResponseDto? Application)>
        CreateAsync(
            int jobId,
            CreateApplicationDto dto,
            string applicantId)
    {
        var job = await _context.Jobs
            .FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null)
        {
            return (false, "Job not found.", null);
        }

        if (!job.IsOpen)
        {
            return (false, "This job is no longer open.", null);
        }

        if (job.PostedById == applicantId)
        {
            return (false, "You cannot apply to your own job.", null);
        }

        var existingApplication = await _context.JobApplications
            .AnyAsync(a =>
                a.JobId == jobId &&
                a.ApplicantId == applicantId);

        if (existingApplication)
        {
            return (
                false,
                "You have already applied to this job.",
                null);
        }

        var application = new JobApplication
        {
            JobId = jobId,
            ApplicantId = applicantId,
            Message = dto.Message,
            Status = "Pending"
        };

        _context.JobApplications.Add(application);

        await _context.SaveChangesAsync();

        // =====================================================
        // CREATE NOTIFICATION FOR JOB OWNER
        // =====================================================

        var notification = await _notificationService.CreateAsync(
            job.PostedById,
            $"A new application was submitted for your job: {job.Title}");

        // =====================================================
        // SEND REAL-TIME NOTIFICATION
        // =====================================================

        await _hubContext.Clients
            .Group($"user-{job.PostedById}")
            .SendAsync(
                "ReceiveNotification",
                new
                {
                    notification.Id,
                    notification.Message,
                    notification.IsRead,
                    notification.CreatedAt
                });

        var response = await GetByIdAsync(application.Id);

        return (
            true,
            "Application submitted successfully.",
            response);
    }

    // =========================================================
    // GET MY APPLICATIONS
    // =========================================================

    public async Task<List<ApplicationResponseDto>> GetMyApplicationsAsync(
        string applicantId)
    {
        return await _context.JobApplications
            .Include(a => a.Job)
            .Include(a => a.Applicant)
            .Where(a => a.ApplicantId == applicantId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new ApplicationResponseDto
            {
                Id = a.Id,
                JobId = a.JobId,
                JobTitle = a.Job != null
                    ? a.Job.Title
                    : string.Empty,
                ApplicantId = a.ApplicantId,
                ApplicantName = a.Applicant != null
                    ? a.Applicant.FullName
                    : string.Empty,
                Message = a.Message,
                Status = a.Status,
                AppliedAt = a.AppliedAt
            })
            .ToListAsync();
    }

    // =========================================================
    // GET APPLICATIONS FOR A JOB
    // =========================================================

    public async Task<(bool Success, string Message, List<ApplicationResponseDto>? Applications)>
        GetJobApplicationsAsync(
            int jobId,
            string userId)
    {
        var job = await _context.Jobs
            .FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null)
        {
            return (false, "Job not found.", null);
        }

        if (job.PostedById != userId)
        {
            return (
                false,
                "You are not authorized to view these applications.",
                null);
        }

        var applications = await _context.JobApplications
            .Include(a => a.Job)
            .Include(a => a.Applicant)
            .Where(a => a.JobId == jobId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new ApplicationResponseDto
            {
                Id = a.Id,
                JobId = a.JobId,
                JobTitle = a.Job != null
                    ? a.Job.Title
                    : string.Empty,
                ApplicantId = a.ApplicantId,
                ApplicantName = a.Applicant != null
                    ? a.Applicant.FullName
                    : string.Empty,
                Message = a.Message,
                Status = a.Status,
                AppliedAt = a.AppliedAt
            })
            .ToListAsync();

        return (true, string.Empty, applications);
    }

    // =========================================================
    // UPDATE APPLICATION STATUS
    // =========================================================

    public async Task<(bool Success, string Message)>
        UpdateStatusAsync(
            int applicationId,
            string status,
            string userId)
    {
        var application = await _context.JobApplications
            .Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application == null)
        {
            return (false, "Application not found.");
        }

        if (application.Job == null ||
            application.Job.PostedById != userId)
        {
            return (
                false,
                "You are not authorized to update this application.");
        }

        var normalizedStatus = status.Trim();

        var allowedStatuses = new[]
        {
            "Pending",
            "Accepted",
            "Rejected"
        };

        var validStatus = allowedStatuses
            .FirstOrDefault(s =>
                s.Equals(
                    normalizedStatus,
                    StringComparison.OrdinalIgnoreCase));

        if (validStatus == null)
        {
            return (
                false,
                "Invalid status. Use Pending, Accepted, or Rejected.");
        }

        application.Status = validStatus;

        await _context.SaveChangesAsync();

        // =====================================================
        // NOTIFY APPLICANT
        // =====================================================

        var notification = await _notificationService.CreateAsync(
            application.ApplicantId,
            $"Your application for '{application.Job.Title}' was {validStatus.ToLower()}.");

        // =====================================================
        // SEND REAL-TIME NOTIFICATION
        // =====================================================

        await _hubContext.Clients
            .Group($"user-{application.ApplicantId}")
            .SendAsync(
                "ReceiveNotification",
                new
                {
                    notification.Id,
                    notification.Message,
                    notification.IsRead,
                    notification.CreatedAt
                });

        return (
            true,
            "Application status updated successfully.");
    }

    // =========================================================
    // DELETE APPLICATION
    // =========================================================

    public async Task<(bool Success, string Message)>
        DeleteAsync(
            int applicationId,
            string applicantId)
    {
        var application = await _context.JobApplications
            .FirstOrDefaultAsync(a =>
                a.Id == applicationId &&
                a.ApplicantId == applicantId);

        if (application == null)
        {
            return (
                false,
                "Application not found or you are not the applicant.");
        }

        if (!application.Status.Equals(
                "Pending",
                StringComparison.OrdinalIgnoreCase))
        {
            return (
                false,
                "Only pending applications can be deleted.");
        }

        _context.JobApplications.Remove(application);

        await _context.SaveChangesAsync();

        return (
            true,
            "Application deleted successfully.");
    }

    // =========================================================
    // GET APPLICATION BY ID
    // =========================================================

    public async Task<ApplicationResponseDto?> GetByIdAsync(
        int applicationId)
    {
        return await _context.JobApplications
            .Include(a => a.Job)
            .Include(a => a.Applicant)
            .Where(a => a.Id == applicationId)
            .Select(a => new ApplicationResponseDto
            {
                Id = a.Id,
                JobId = a.JobId,
                JobTitle = a.Job != null
                    ? a.Job.Title
                    : string.Empty,
                ApplicantId = a.ApplicantId,
                ApplicantName = a.Applicant != null
                    ? a.Applicant.FullName
                    : string.Empty,
                Message = a.Message,
                Status = a.Status,
                AppliedAt = a.AppliedAt
            })
            .FirstOrDefaultAsync();
    }
}