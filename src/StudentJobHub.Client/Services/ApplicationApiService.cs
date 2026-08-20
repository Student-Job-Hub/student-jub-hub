using System.Net.Http.Json;
using StudentJobHub.Client.Models;

namespace StudentJobHub.Client.Services;

public class ApplicationApiService
{
    private readonly HttpClient _httpClient;

    public ApplicationApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> ApplyAsync(
        int jobId,
        CreateApplicationModel model)
    {
        return await _httpClient.PostAsJsonAsync(
            $"api/applications/{jobId}",
            model);
    }

    public async Task<List<ApplicationModel>> GetMyApplicationsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ApplicationModel>>(
            "api/applications/my")
            ?? new List<ApplicationModel>();
    }

    public async Task<ApplicationModel?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ApplicationModel>(
            $"api/applications/{id}");
    }

    public async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        return await _httpClient.DeleteAsync(
            $"api/applications/{id}");
    }
}