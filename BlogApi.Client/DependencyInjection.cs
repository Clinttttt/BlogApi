using BlogApi.Client.Interface;
using BlogApi.Client.Security;
using BlogApi.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlogApi.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddPersistence(services, configuration);
            AddAuthentication(services);
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Register callback service
            services.AddScoped<GoogleAuthCallbackService>();
            services.AddScoped<ProtectedLocalStorage>();
            // Register delegating handler as SCOPED (same lifetime as ProtectedLocalStorage)
            services.AddScoped<AuthorizationDelegatingHandler>();

            // Auth client (no delegating handler)
            services.AddHttpClient<IAuthClientService, AuthClientService>("AuthClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7096");
            });

            // API client (with delegating handler for authenticated requests)
            services.AddHttpClient("Api", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7096");
            }).AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            // Post service
            services.AddScoped<IPostClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new PostClientService(httpClient);
            });

            // Category service
            services.AddScoped<ICategoryClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new CategoryClientService(httpClient);
            });

            // Tag service
            services.AddScoped<ITagClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new TagClientService(httpClient);
            });

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            // Register custom AuthStateProvider as scoped
            services.AddScoped<AuthStateProvider>();

            // Register it as the AuthenticationStateProvider implementation
            services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<AuthStateProvider>());

            // Add authorization core services
            services.AddAuthorizationCore();

            return services;
        }
    }
}