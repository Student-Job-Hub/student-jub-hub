using System.Net.Http.Json;
using StudentJobHub.Client.Models;

namespace StudentJobHub.Client.Services;

public class ServiceApiService
{
    private readonly HttpClient _httpClient;

    public ServiceApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ServiceModel>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ServiceModel>>("api/services")
            ?? new List<ServiceModel>();
    }

    public async Task<List<ServiceModel>> GetMyServicesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ServiceModel>>("api/services/my")
            ?? new List<ServiceModel>();
    }

    public async Task<ServiceModel?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ServiceModel>($"api/services/{id}");
    }

    public async Task<HttpResponseMessage> CreateAsync(CreateServiceModel model)
    {
        return await _httpClient.PostAsJsonAsync("api/services", model);
    }

    public async Task<HttpResponseMessage> UpdateAsync(int id, CreateServiceModel model)
    {
        return await _httpClient.PutAsJsonAsync($"api/services/{id}", model);
    }

    public async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        return await _httpClient.DeleteAsync($"api/services/{id}");
    }
}
