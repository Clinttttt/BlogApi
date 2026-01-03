using BlogApi.Application.Dtos;
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

namespace BlogApi.Application.Queries.User.GetUserInfo
{
    public class GetUserInfoQueryHandler(IAppDbContext context) : IRequestHandler<GetUserInfoQuery, Result<UserInfoDto>>
    {
        public async Task<Result<UserInfoDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(s => s.ExternalLogins)
                .Include(s => s.UserInfo)
                .AsNoTracking()
                .Where(s => s.Id == request.UserId)
                .Select(s => new UserInfoDto
                {
                    FullName = s.UserInfo != null ? s.UserInfo.FullName : s.UserName,

                    ProfilePhoto = s.UserInfo != null && s.UserInfo.Photo != null && s.UserInfo.Photo.Length > 0
                    ? $"data:{s.UserInfo.PhotoContentType};base64,{Convert.ToBase64String(s.UserInfo.Photo)}"
                    : s.ExternalLogins.Select(x => x.ProfilePhotoUrl).FirstOrDefault(),

                    Bio = s.UserInfo != null ? s.UserInfo.Bio : "Add a short bio",
                    CreatedAt = s.ExternalLogins.Select(s => s.LinkedAt).FirstOrDefault(),

                }).FirstOrDefaultAsync();
            if (user is null)
                return Result<UserInfoDto>.NoContent();
            return Result<UserInfoDto>.Success(user);

        }
    }
}
