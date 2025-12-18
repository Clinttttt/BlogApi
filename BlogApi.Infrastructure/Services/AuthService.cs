using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Services
{
    public class AuthService(ITokenService tokenService, AppDbContext context , IGoogleTokenValidator tokenValidator) : IAuthService
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
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return Result<TokenResponseDto>.Unauthorized();
            }
            var token = await tokenService.CreateTokenResponse(user);
            return Result<TokenResponseDto>.Success(token);
        }
        public async Task<Result<TokenResponseDto>> GoogleLogin(string IdToken)
        {
            var GoogleUser = await tokenValidator.ValidateAsync(IdToken);
            if(GoogleUser is null) 
                return Result<TokenResponseDto>.Unauthorized();

            var user = await context.Users
                .Include(s => s.ExternalLogins)          
                .FirstOrDefaultAsync(s => s.Email == GoogleUser.Value!.Email);
            if(user is null)
            {
                user = new User
                {
                    UserName = GoogleUser.Value!.Email.Split('@')[0],
                    Email = GoogleUser.Value.Email,
                    PasswordHash = null!
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();   
            }
            var externalLogin = user.ExternalLogins
        .FirstOrDefault(el => el.Provider == "Google" && el.ProviderId == GoogleUser.Value!.Sub);

            if(externalLogin is null)
            {
                user.ExternalLogins.Add(new ExternalLogin
                {   
                    Provider = "Google",
                    ProviderId = GoogleUser.Value!.Sub,
                    LinkedAt = DateTime.UtcNow.AddHours(8)
                });
               await context.SaveChangesAsync();
            }
            var token = await tokenService.CreateTokenResponse(user);
            return Result<TokenResponseDto>.Success(token);
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
