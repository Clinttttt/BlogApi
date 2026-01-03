
using BlogApi.Application.Common.Interfaces;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Persistence;
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
          
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddHttpClient<IGoogleTokenValidator, GoogleTokenValidator>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, SendGridEmailService>();
            services.AddScoped<IPostRespository, PostRespository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IUserRespository, UserRespository>();
            services.AddScoped<ITagRespository, TagRespository>();
            services.AddScoped<ICategoryRespository, CategoryRespository>();
            return services;
        }
    }
}
