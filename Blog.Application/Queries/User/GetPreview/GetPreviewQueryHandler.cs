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
    public class GetPreviewQueryHandler(IAppDbContext context) : IRequestHandler<GetPreviewQuery, Result<UserProfileDto>>
    {
        public async Task<Result<UserProfileDto>> Handle(GetPreviewQuery request, CancellationToken cancellationToken)
        {
            var photo = await context.Users
                 .AsNoTracking()
                 .Include(s=> s.UserInfo)
                 .Include(s=> s.ExternalLogins)
                 .Where(s=> s.Id == request.UserId)
                 .Select(c => new UserProfileDto
                 {
                     PhotoUrl = c.UserInfo != null && c.UserInfo.Photo != null && c.UserInfo.Photo.Length > 0
                         ? $"data:{c.UserInfo.PhotoContentType};base64,{Convert.ToBase64String(c.UserInfo.Photo)}"
                         : c.ExternalLogins
                           .FirstOrDefault(el => el.Provider == "Google" && el.ProfilePhotoBytes != null) != null
                         ? $"data:image/jpeg;base64,{Convert.ToBase64String(c.ExternalLogins.First(el => el.Provider == "Google").ProfilePhotoBytes!)}"
                         : string.Empty,

                 }).FirstOrDefaultAsync();
            if (photo is null)
                return Result<UserProfileDto>.NotFound();
            return Result<UserProfileDto>.Success(photo);
                            
        }
    }
}
