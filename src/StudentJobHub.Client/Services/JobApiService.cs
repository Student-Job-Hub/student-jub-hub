using System.Net.Http.Json;
using StudentJobHub.Client.Models;

namespace StudentJobHub.Client.Services;

public class JobApiService
{
    private readonly HttpClient _httpClient;

    public JobApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<JobModel>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<JobModel>>(
                   "api/Jobs")
               ?? new List<JobModel>();
    }

    public async Task<JobModel?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<JobModel>(
            $"api/Jobs/{id}");
    }

    public async Task<HttpResponseMessage> CreateAsync(
        CreateJobModel model)
    {
        return await _httpClient.PostAsJsonAsync(
            "api/Jobs",
            model);
    }

    public async Task<HttpResponseMessage> UpdateAsync(
        int id,
        CreateJobModel model)
    {
        return await _httpClient.PutAsJsonAsync(
            $"api/Jobs/{id}",
            model);
    }

    public async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        return await _httpClient.DeleteAsync(
            $"api/Jobs/{id}");
    }

    public async Task<HttpResponseMessage> CloseAsync(int id)
    {
        return await _httpClient.PatchAsync(
            $"api/Jobs/{id}/close",
            null);
    }
}