using Blog.Client.Components;
using Blog.Infrastructure.Hubs;
using BlogApi.Client;
using BlogApi.Client.Middleware;
using BlogApi.Client.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSignalR();
builder.Services.Configure<CircuitOptions>(options =>
{
    options.DetailedErrors = true;
});


builder.Services.AddServices(builder.Configuration);


var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
  
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.Middleware();
app.UseAntiforgery();
app.MapHub<PostHub>("/posthub");
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

