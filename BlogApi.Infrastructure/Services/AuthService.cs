using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Services
{
    public class AuthService(ITokenService tokenService, AppDbContext context, IGoogleTokenValidator tokenValidator, IHttpClientFactory _httpClient, ILogger<AuthService> _logger) : IAuthService
    {
        public async Task<Result<AuthResult>> Register(UserDto request)
        {
            if (await context.Users.AnyAsync(s => s.UserName == request.UserName))
            {
                return Result<AuthResult>.Conflict();
            }
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return Result<AuthResult>.Failure();
            }
            var user = new User();
            var passwordHashed = new PasswordHasher<User>()
                .HashPassword(user, request.Password);
            user.UserName = request.UserName;
            user.PasswordHash = passwordHashed;
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var filter = new AuthResult
            {
                UserName = request.UserName,
                CreatedAt = DateTime.UtcNow.AddHours(8),
            };

            return Result<AuthResult>.Success(filter);
        }
        public async Task<Result<TokenResponseDto>> Login(UserDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(s => s.UserName == request.UserName);
            if (user == null)
            {
                return Result<TokenResponseDto>.Unauthorized();
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash!, request.Password) == PasswordVerificationResult.Failed)
            {
                return Result<TokenResponseDto>.Unauthorized();
            }
            var token = await tokenService.CreateTokenResponse(user);
            return token;
        }
        public async Task<Result<TokenResponseDto>> GoogleLogin(string IdToken)
        {
            var GoogleUser = await tokenValidator.ValidateAsync(IdToken);
            if (GoogleUser is null)
                return Result<TokenResponseDto>.Unauthorized();

          using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var user = await context.Users
              .Include(s => s.ExternalLogins)
              .FirstOrDefaultAsync(s => s.Email == GoogleUser.Value!.Email);
                if (user is null)
                {
                    user = new User
                    {
                        UserName = GoogleUser.Value!.Email.Split('@')[0],
                        Email = GoogleUser.Value.Email,
                        PasswordHash = null!,
                        Role = "Author"
                    };
                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                }
                var externalLogin = user.ExternalLogins
            .FirstOrDefault(el => el.Provider == "Google" && el.ProviderId == GoogleUser.Value!.Sub);

                if (externalLogin is null)
                {
                    byte[]? photoBytes = null;
                    if (!string.IsNullOrEmpty(GoogleUser.Value!.Picture))
                    {
                        try
                        {
                            using var httpClient = _httpClient.CreateClient();
                            photoBytes = await httpClient.GetByteArrayAsync(GoogleUser.Value.Picture);
                        }
                        catch (Exception ex)
                        {
                            photoBytes = null;
                            _logger.LogWarning(ex, "Failed to download profile photo from {Url}", GoogleUser.Value.Picture);
                        }
                    }
                    user.ExternalLogins.Add(new ExternalLogin
                    {
                        Provider = "Google",
                        ProviderId = GoogleUser.Value!.Sub,
                        LinkedAt = DateTime.UtcNow.AddHours(8),
                        ProfilePhotoUrl = GoogleUser.Value.Picture,
                        ProfilePhotoBytes = photoBytes

                    });
                    await context.SaveChangesAsync();
                }
                var token = await tokenService.CreateTokenResponse(user);
                await transaction.CommitAsync();
                return token;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<Result<TokenResponseDto>> RefreshToken(RefreshTokenDto request)
        {
            var user = await tokenService.ValidateRefreshToken(request.UserId, request.RefreshToken);
            if (user is null)
                return Result<TokenResponseDto>.NotFound();
            return await tokenService.CreateTokenResponse(user);

        }

        public async Task<Result<bool>> Logout(Guid Id)
        {
            var user = await context.Users.FindAsync(Id);
            if (user == null)
                return Result<bool>.NotFound();
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(-1);
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);

        }
    }
}
