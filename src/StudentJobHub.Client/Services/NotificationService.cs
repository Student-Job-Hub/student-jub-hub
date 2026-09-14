using System.Net.Http.Json;
using StudentJobHub.Client.Models;

namespace StudentJobHub.Client.Services;

public class NotificationService
{
    private readonly HttpClient _httpClient;

    public NotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NotificationModel>> GetNotificationsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<NotificationModel>>("api/notifications")
            ?? new List<NotificationModel>();
    }

    public async Task<HttpResponseMessage> MarkAsReadAsync(int id)
    {
        return await _httpClient.PatchAsync($"api/notifications/{id}/read", null);
    }

    public async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        return await _httpClient.DeleteAsync($"api/notifications/{id}");
    }
}
