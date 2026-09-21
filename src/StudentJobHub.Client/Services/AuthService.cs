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

    public event Action? OnAuthStateChanged;

    public async Task InitializeAsync()
    {
        await GetTokenAsync();
    }

    public async Task<string?> GetTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(Token))
        {
            try
            {
                Token = await _jsRuntime.InvokeAsync<string?>("authStorage.getToken");
            }
            catch
            {
                // In case JS interop is not ready yet during pre-rendering
            }
        }

        return Token;
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
            OnAuthStateChanged?.Invoke();
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
            OnAuthStateChanged?.Invoke();
        }

        return result;
    }

    public async Task LogoutAsync()
    {
        Token = null;
        try
        {
            await _jsRuntime.InvokeVoidAsync("authStorage.removeToken");
        }
        catch
        {
            // Ignore if JS interop error during logout
        }
        OnAuthStateChanged?.Invoke();
    }
}