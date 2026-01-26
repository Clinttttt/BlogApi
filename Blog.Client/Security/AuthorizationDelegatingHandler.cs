using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Client.Security
{
    public class AuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<AuthorizationDelegatingHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationDelegatingHandler(
            ILogger<AuthorizationDelegatingHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext != null &&
                    httpContext.Request.Cookies.TryGetValue("AccessToken", out var token) &&
                    !string.IsNullOrWhiteSpace(token))
                {
                    token = token.Trim().Trim('"').Trim();
                  

                    request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attaching auth token");
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
