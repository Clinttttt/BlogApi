using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace BlogApi.Client.Security
{
    public class AuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<AuthorizationDelegatingHandler> _logger;
        private readonly IJSRuntime _js;
        private static string? _cachedToken;
        private static DateTime _tokenCacheExpiry = DateTime.MinValue;
        private static readonly object _cacheLock = new();

        public AuthorizationDelegatingHandler(
            ILogger<AuthorizationDelegatingHandler> logger,
            IJSRuntime js)
        {
            _logger = logger;
            _js = js;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            try
            {
            
                string? token = GetCachedToken();
                
              
                if (string.IsNullOrEmpty(token))
                {                
                    token = await _js.InvokeAsync<string?>("localStorage.getItem", "AccessToken");
                                   
                    if (!string.IsNullOrEmpty(token))
                    {
                        CacheToken(token);
                    }
                }

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (InvalidOperationException)
            {
              
                _logger.LogDebug("JSInterop not available during prerendering");
            }
            catch (JSException jsEx)
            {
                _logger.LogWarning(jsEx, "JavaScript error while retrieving token");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving authentication token");
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private string? GetCachedToken()
        {
            lock (_cacheLock)
            {
                return !string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenCacheExpiry
                    ? _cachedToken
                    : null;
            }
        }

        public static void CacheToken(string token)
        {
            lock (_cacheLock)
            {
                _cachedToken = token;
                _tokenCacheExpiry = DateTime.UtcNow.AddDays(1);
            }
        }

        public static void ClearCache()
        {
            lock (_cacheLock)
            {
                _cachedToken = null;
                _tokenCacheExpiry = DateTime.MinValue;
            }
        }
    }
}