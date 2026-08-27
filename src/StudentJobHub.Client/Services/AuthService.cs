using System.Net.Http.Json;
using Microsoft.JSInterop;
using StudentJobHub.Client.Models;

namespace StudentJobHub.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public string? Token { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

    public async Task InitializeAsync()
    {
        Token = await _jsRuntime.InvokeAsync<string?>("authStorage.getToken");
    }

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
            await _jsRuntime.InvokeVoidAsync("authStorage.setToken", Token);
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
            await _jsRuntime.InvokeVoidAsync("authStorage.setToken", Token);
        }

        return result;
    }

    public async Task LogoutAsync()
    {
        Token = null;
        await _jsRuntime.InvokeVoidAsync("authStorage.removeToken");
    }
}