using System.Net.Http.Headers;

namespace StudentJobHub.Client.Services;

public class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly AuthService _authService;

    public JwtAuthorizationHandler(AuthService authService)
    {
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _authService.Token;

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(
            request,
            cancellationToken);
    }
}
