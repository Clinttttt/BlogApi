using AutoMapper;
using Blog.Api.Extensions;
using Blog.Api.Middleware;
using Blog.Application;
using Blog.Application.Abstractions;
using Blog.Application.Behaviors;
using Blog.Application.Common.Interfaces;
using Blog.Infrastructure.Services;
using Blog.Infrastructure.SignalR.Comments;
using Blog.Infrastructure.SignalR.Notifications;
using Blog.Infrastructure.SignalR.Posts;
using BlogApi.Application;
using BlogApi.Infrastructure;
using CQRSMEDIATR.Api;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);




builder.ConfigureServices();
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true) 
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

builder.Configuration
       .AddJsonFile("appsettings.json", optional: false)
       .AddJsonFile("appsettings.Local.json", optional: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
       {
           {
               new OpenApiSecurityScheme
               {
                   Reference = new OpenApiReference
                   {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                   }
               },
               new string[] {}
           }
       });
});

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(
        new DirectoryInfo("/root/.aspnet/DataProtection-Keys"))
    .SetApplicationName("BlogApp");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}


var urls = builder.Configuration["ASPNETCORE_URLS"] ?? string.Empty;
if (urls.Contains("https", StringComparison.OrdinalIgnoreCase))
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseCors("AllowBlazor");
app.UseAuthentication();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.MapHub<PostHub>("/hubs/posts");
app.MapHub<CommentHub>("/hubs/comments");
app.MapHub<NotificatonHub>("/hubs/notifications");

app.Run();
