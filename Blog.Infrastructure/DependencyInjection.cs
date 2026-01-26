
using Blog.Application.Abstractions;
using Blog.Application.Common.Interfaces.Repositories;
using Blog.Application.Common.Interfaces.Services;
using Blog.Application.Common.Interfaces.SignalR;
using Blog.Application.Common.Interfaces.Utilities;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Respository;
using Blog.Infrastructure.Services;
using Blog.Infrastructure.SignalR.Comments;
using Blog.Infrastructure.SignalR.Notifications;
using Blog.Infrastructure.SignalR.Posts;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Respository;
using BlogApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");

         
            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                connectionString = connectionString?.Replace("localhost", "blog.database");
                Console.WriteLine($"🐳 Running in Docker - Using connection: {connectionString}");
            }

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    connectionString!, 
                    npgsqlOptions =>   
                    {
                        npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }));




            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddHttpClient<IGoogleTokenValidator, GoogleTokenValidator>();
            services.AddScoped<IFilterBuilder, FilterBuilder>();


            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEmailService, SendGridEmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<ICacheService, MemoryCacheService>();


            services.AddScoped<IUserRespository, UserRespository>();
            services.AddScoped<IPostRespository, PostRespository>();
            services.AddScoped<INotificationRespository, NotificationRespository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<ITagRespository, TagRespository>();
            services.AddScoped<ICategoryRespository, CategoryRespository>();


            services.AddScoped<IPostHubService, PostHubService>();
            services.AddScoped<INotificatonHubService, NotificatonHubService>();
            services.AddScoped<ICommentHubService, CommentHubService>();


            return services;
        }
    }
}
