using BlogApi.Client.Interface;
using BlogApi.Client.Security;
using BlogApi.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlogApi.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddPersistence(services, configuration);
            AddCustomAuthentication(services);
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.AddScoped<GoogleAuthCallbackService>();
            services.AddScoped<ProtectedLocalStorage>();        
            services.AddScoped<AuthorizationDelegatingHandler>();


            services.AddHttpClient<IAuthClientService, AuthClientService>("AuthClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7096");
            });
       
            services.AddHttpClient("Api", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7096");
            }).AddHttpMessageHandler<AuthorizationDelegatingHandler>();       
            services.AddScoped<IPostClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("Api");
                return new PostClientService(httpClient);
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

                // for page requests, redirect to login
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        });

            services.AddAuthorizationCore();
          

            services.AddScoped<AuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<AuthStateProvider>());

            return services;
        }
    }
}