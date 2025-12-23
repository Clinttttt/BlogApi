using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace BlogApi.Client.Security
{
    public class AuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly ILogger<AuthorizationDelegatingHandler> _logger;

        private static string? _cachedToken;
        private static DateTime _tokenCacheExpiry = DateTime.MinValue;
        private static readonly object _cacheLock = new();

        public AuthorizationDelegatingHandler(
            ProtectedLocalStorage localStorage,
            ILogger<AuthorizationDelegatingHandler> logger)
        {
            _localStorage = localStorage;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string? token = GetCachedToken() ?? await RetrieveTokenFromStorageAsync();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        private async Task<string?> RetrieveTokenFromStorageAsync()
        {
            try
            {
                var tokenResult = await _localStorage.GetAsync<string>("AccessToken");

                if (tokenResult.Success && !string.IsNullOrEmpty(tokenResult.Value))
                {
                    CacheToken(tokenResult.Value);
                    return tokenResult.Value;
                }
            }
            catch (InvalidOperationException)
            {
                return GetCachedToken();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving token");
            }

            return null;
        }

        public static void CacheToken(string token)
        {
            lock (_cacheLock)
            {
                _cachedToken = token;
                _tokenCacheExpiry = DateTime.UtcNow.AddMinutes(5);
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