using BlogApi.Client;
using BlogApi.Client.Components;
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

builder.Services.Configure<CircuitOptions>(options =>
{
    options.DetailedErrors = true;
});


builder.Services.AddServices(builder.Configuration);


var app = builder.Build();


app.MapPost("/api/auth/set-cookie", async (
    [FromBody] string token,
    HttpContext context,
    AuthStateProvider authStateProvider) =>
{
    try
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var claims = jwt.Claims.Select(c =>
        {
            if (c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                return new Claim(ClaimTypes.Role, c.Value);
            if (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                return new Claim(ClaimTypes.Name, c.Value);
            if (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                return new Claim(ClaimTypes.NameIdentifier, c.Value);
            return c;
        }).ToList();

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = jwt.ValidTo
            });

        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest();
    }
});

app.MapPost("/api/auth/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok();
});
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
  
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
