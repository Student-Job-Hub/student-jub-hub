using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace StudentJobHub.Client.Services;

public class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly AuthService _authService;
    private readonly IJSRuntime _jsRuntime;

    public JwtAuthorizationHandler(AuthService authService, IJSRuntime jsRuntime)
    {
        _authService = authService;
        _jsRuntime = jsRuntime;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _authService.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            try
            {
                token = await _jsRuntime.InvokeAsync<string?>("authStorage.getToken");
            }
            catch
            {
                // Fallback catch if JS runtime fails
            }
        }

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(
            request,
            cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _authService.LogoutAsync();
        }

        return response;
    }
}