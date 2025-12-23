using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace BlogApi.Client.Security
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localStorage;
        private bool _isInitialized = false;

        public AuthStateProvider(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!_isInitialized)
            {
                return CreateAnonymousState();
            }

            try
            {
                var tokenResult = await _localStorage.GetAsync<string>("AccessToken");

                if (!tokenResult.Success || string.IsNullOrWhiteSpace(tokenResult.Value))
                {
                    return CreateAnonymousState();
                }

                var parts = tokenResult.Value.Split('.');
                if (parts.Length != 3)
                {
                    return CreateAnonymousState();
                }

                try
                {
                    var payload = parts[1];

                    switch (payload.Length % 4)
                    {
                        case 2: payload += "=="; break;
                        case 3: payload += "="; break;
                    }

                    var jsonBytes = Convert.FromBase64String(payload);
                    var jsonString = System.Text.Encoding.UTF8.GetString(jsonBytes);

                    var json = System.Text.Json.JsonDocument.Parse(jsonString);
                    var claims = new List<Claim>();

                    foreach (var property in json.RootElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind == System.Text.Json.JsonValueKind.String)
                        {
                            claims.Add(new Claim(property.Name, property.Value.GetString() ?? ""));
                        }
                        else if (property.Value.ValueKind == System.Text.Json.JsonValueKind.Number)
                        {
                            claims.Add(new Claim(property.Name, property.Value.ToString()));
                        }
                    }

                    var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
                    if (expClaim != null && long.TryParse(expClaim.Value, out var exp))
                    {
                        var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;

                        if (expirationTime < DateTime.UtcNow)
                        {
                            await ClearAuthDataAsync();
                            return CreateAnonymousState();
                        }
                    }

                    var identity = new ClaimsIdentity(claims, "jwt");
                    var user = new ClaimsPrincipal(identity);

                    return new AuthenticationState(user);
                }
                catch
                {
                    await ClearAuthDataAsync();
                    return CreateAnonymousState();
                }
            }
            catch
            {
                return CreateAnonymousState();
            }
        }

        public async Task InitializeAsync()
        {
            _isInitialized = true;
            await MarkUserAsAuthenticated();
        }

        public async Task MarkUserAsAuthenticated()
        {
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await ClearAuthDataAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(CreateAnonymousState()));
        }

        private async Task ClearAuthDataAsync()
        {
            try
            {
                await _localStorage.DeleteAsync("AccessToken");
                await _localStorage.DeleteAsync("RefreshToken");
            }
            catch
            {
                // Silent fail
            }
        }

        private AuthenticationState CreateAnonymousState()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymous);
        }

        public async Task<string?> GetUserIdAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false
                ? authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                : null;
        }

        public async Task<string?> GetUserNameAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false
                ? authState.User.FindFirst(ClaimTypes.Name)?.Value
                : null;
        }
    }
}