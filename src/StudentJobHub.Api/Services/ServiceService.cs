using Microsoft.EntityFrameworkCore;
using StudentJobHub.Api.Data;
using StudentJobHub.Api.DTOs.Services;
using StudentJobHub.Api.Models;

namespace StudentJobHub.Api.Services;

public class ServiceService
{
    private readonly ApplicationDbContext _context;

    public ServiceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponseDto> CreateAsync(
        CreateServiceDto dto,
        string providerId)
    {
        var service = new Service
        {
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Category,
            Price = dto.Price,
            ProviderId = providerId
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        return await GetResponseAsync(service.Id)
            ?? throw new InvalidOperationException(
                "Failed to retrieve the created service.");
    }

    public async Task<List<ServiceResponseDto>> GetAllAsync()
    {
        return await _context.Services
            .Include(s => s.Provider)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new ServiceResponseDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Category = s.Category,
                Price = s.Price,
                ProviderId = s.ProviderId,
                ProviderName = s.Provider != null
                    ? s.Provider.FullName
                    : string.Empty,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<ServiceResponseDto>> GetByProviderIdAsync(string providerId)
    {
        return await _context.Services
            .Include(s => s.Provider)
            .Where(s => s.ProviderId == providerId)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new ServiceResponseDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Category = s.Category,
                Price = s.Price,
                ProviderId = s.ProviderId,
                ProviderName = s.Provider != null
                    ? s.Provider.FullName
                    : string.Empty,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ServiceResponseDto?> GetByIdAsync(int id)
    {
        return await GetResponseAsync(id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateServiceDto dto,
        string providerId)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s =>
                s.Id == id &&
                s.ProviderId == providerId);

        if (service == null)
        {
            return false;
        }

        service.Title = dto.Title;
        service.Description = dto.Description;
        service.Category = dto.Category;
        service.Price = dto.Price;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        string providerId)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s =>
                s.Id == id &&
                s.ProviderId == providerId);

        if (service == null)
        {
            return false;
        }

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<ServiceResponseDto?> GetResponseAsync(int id)
    {
        return await _context.Services
            .Include(s => s.Provider)
            .Where(s => s.Id == id)
            .Select(s => new ServiceResponseDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Category = s.Category,
                Price = s.Price,
                ProviderId = s.ProviderId,
                ProviderName = s.Provider != null
                    ? s.Provider.FullName
                    : string.Empty,
                CreatedAt = s.CreatedAt
            })
            .FirstOrDefaultAsync();
    }
}