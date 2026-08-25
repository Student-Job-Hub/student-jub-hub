using System.Net.Http.Json;
using StudentJobHub.Client.Models;

namespace StudentJobHub.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string? Token { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

    public async Task<AuthResponse?> LoginAsync(LoginModel model)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/auth/login",
            model);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content
            .ReadFromJsonAsync<AuthResponse>();

        if (result != null && !string.IsNullOrWhiteSpace(result.Token))
        {
            Token = result.Token;
        }

        return result;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterModel model)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/auth/register",
            model);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content
            .ReadFromJsonAsync<AuthResponse>();

        if (result != null && !string.IsNullOrWhiteSpace(result.Token))
        {
            Token = result.Token;
        }

        return result;
    }

    public void Logout()
    {
        Token = null;
    }
}