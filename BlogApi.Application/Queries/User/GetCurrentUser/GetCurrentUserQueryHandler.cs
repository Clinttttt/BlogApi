using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.User.CurrentUser;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.User.GetCurrentUser
{
    public class GetCurrentUserQueryHandler(IAppDbContext context) : IRequestHandler<GetCurrentUserQuery, Result<UserProfileDto>>
    {
        public async Task<Result<UserProfileDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var photo = await context.Users
                 .AsNoTracking()
                 .Include(s=> s.UserInfo)
                 .Where(s=> s.Id == request.UserId)
                 .Select(s => new UserProfileDto
                 {
                     PhotoUrl = s.UserInfo != null && s.UserInfo.Photo != null && s.UserInfo.Photo.Length > 0
                    ? $"data:{s.UserInfo.PhotoContentType};base64,{Convert.ToBase64String(s.UserInfo.Photo)}"
                    : s.ExternalLogins.Select(x => x.ProfilePhotoUrl).FirstOrDefault(),

                 }).FirstOrDefaultAsync();
            if (photo is null)
                return Result<UserProfileDto>.NotFound();
            return Result<UserProfileDto>.Success(photo);
                            
        }
    }
}
