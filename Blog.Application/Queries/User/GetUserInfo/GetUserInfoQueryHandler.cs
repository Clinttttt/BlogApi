using BlogApi.Application.Common.Interfaces;
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
    public class GetUserInfoQueryHandler(IUserRespository respository) : IRequestHandler<GetUserInfoQuery, Result<UserInfoDto>>
    {
        public async Task<Result<UserInfoDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {

            var user = await respository.Get(filter: s => s.Id == request.UserId, cancellationToken);

            var dto = new UserInfoDto
            {
                FullName = user.UserInfo != null ? user.UserInfo.FullName : user.UserName,
                ProfilePhoto = user.UserInfo != null && user.UserInfo.Photo != null && user.UserInfo.Photo.Length > 0
                    ? $"data:{user.UserInfo.PhotoContentType};base64,{Convert.ToBase64String(user.UserInfo.Photo)}"
                    : user.ExternalLogins.Select(x => x.ProfilePhotoUrl).FirstOrDefault(),
                Bio = user.UserInfo != null ? user.UserInfo.Bio : "Add a short bio",
                CreatedAt = user.ExternalLogins.Select(s => s.LinkedAt).FirstOrDefault(),
            };
          
            if (dto is null)
                return Result<UserInfoDto>.NoContent();
            return Result<UserInfoDto>.Success(dto);

        }
    }
}
                   