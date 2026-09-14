using System.Net.Http.Json;
using StudentJobHub.Client.Models;

namespace StudentJobHub.Client.Services;

public class UserApiService
{
    private readonly HttpClient _httpClient;

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserModel?> GetCurrentUserAsync()
    {
        return await _httpClient.GetFromJsonAsync<UserModel>("api/users/me");
    }

    public async Task<HttpResponseMessage> UpdateCurrentUserAsync(UpdateProfileModel model)
    {
        return await _httpClient.PutAsJsonAsync("api/users/me", model);
    }
}
