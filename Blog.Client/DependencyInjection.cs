using Blog.Client.Interface;
using Blog.Client.Realtime;
using Blog.Client.Security;
using Blog.Client.Services;
using Blog.Client.State;
using BlogApi.Client.Common.Auth;
using BlogApi.Client.Interface;
using BlogApi.Client.Security;
using BlogApi.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlogApi.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddPersistence(services, configuration);
            AddCustomAuthentication(services);

            services.AddScoped<PostViewState>();
            services.AddScoped<NotificationState>();
            services.AddScoped<PostHubClient>();
            services.AddScoped<CommenthubClient>();
            services.AddScoped<NotificationHubClient>();

            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<GoogleAuthCallbackService>();
            services.AddScoped<AuthorizationDelegatingHandler>();
            services.AddScoped<RefreshTokenDelegatingHandler>();

            var apiBase = configuration["LocalHost"]!;

            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                apiBase = configuration["DockerHost"]!;

            }        
         
            services.AddHttpClient<IAuthClientService, AuthClientService>("AuthClient", client =>
            {
                client.BaseAddress = new Uri(apiBase);
            });

            services.AddHttpClient("Api", client =>
            {
                client.BaseAddress = new Uri(apiBase);
            })
                .AddHttpMessageHandler<RefreshTokenDelegatingHandler>()
                .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationDelegatingHandler>());

            
            services.AddScoped<IPostClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new PostClientService(httpClient);
            });

            services.AddScoped<IPostInteractionsClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new PostInteractionsClientService(httpClient);
            });

            services.AddScoped<ICommentsClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new CommentsClientService(httpClient);
            });

            services.AddScoped<IPostModerationClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new PostModerationClientService(httpClient);
            });

            services.AddScoped<IFeaturedPostsClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new FeaturedPostsClientService(httpClient);
            });

            services.AddScoped<IPostAnalyticsClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new PostAnalyticsClientService(httpClient);
            });

          
            services.AddScoped<ICategoryClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new CategoryClientService(httpClient);
            });

            services.AddScoped<ITagClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new TagClientService(httpClient);
            });

            services.AddScoped<IUserClientServices>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new UserClientServices(httpClient);
            });

            services.AddScoped<INotificationClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new NotificationClientService(httpClient);
            });

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/";
                    options.AccessDeniedPath = "/access-denied";
                    options.Events.OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api"))
                        {
                            context.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorizationCore();
            services.AddScoped<AuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<AuthStateProvider>());

            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}