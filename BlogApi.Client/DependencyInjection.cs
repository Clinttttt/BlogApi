using BlogApi.Client.Interface;
using BlogApi.Client.Services;
using BlogApi.Domain.Entities;
using BlogApi.Infrastructure.Persistence;

namespace BlogApi.Client
{
    public static class DependencyInjection
    {


        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddPersistence(services, configuration);
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<GoogleAuthCallbackService>();
            services.AddScoped<AuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthClientService, AuthClientService>("AuthClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7096");
            });
            
            services.AddHttpClient("Api", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7096");
            }).AddHttpMessageHandler<AuthorizationDelegatingHandler>();
            return services;
        }
    }
}

/*
How they work together:

User logs in with Google
    ↓
IAuthClientService (uses "AuthClient") - NO handler
    ↓
Token stored in localStorage
    ↓
User makes API call to get posts
    ↓
HttpClient "Api" (WITH handler)
    ↓
Handler reads token from localStorage
    ↓
Adds "Authorization: Bearer {token}" header
    ↓
Request sent to API ✅
```

## If token expires:
```
Handler checks token
    ↓
Token expired or missing
    ↓
Handler uses IHttpClientFactory.CreateClient("AuthClient")
    ↓
Calls refresh endpoint (NO handler - no circular dependency!)
    ↓
Gets new token
    ↓
Stores new token
    ↓
Adds to current request
    ↓
Request continues ✅ 

 */