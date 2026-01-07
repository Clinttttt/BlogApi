using BlogApi.Application.Common.Interfaces;
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
    public class GetPreviewQueryHandler(IUserRespository respository) : IRequestHandler<GetPreviewQuery, Result<UserProfileDto>>
    {
        public async Task<Result<UserProfileDto>> Handle(GetPreviewQuery request, CancellationToken cancellationToken)
        {
            var user = await respository.Get(filter: s => s.Id == request.UserId,cancellationToken);

            var photo = new UserProfileDto
            {
                PhotoUrl = user.UserInfo != null && user.UserInfo.Photo != null && user.UserInfo.Photo.Length > 0
                         ? $"data:{user.UserInfo.PhotoContentType};base64,{Convert.ToBase64String(user.UserInfo.Photo)}"
                         : user.ExternalLogins
                           .FirstOrDefault(el => el.Provider == "Google" && el.ProfilePhotoBytes != null) != null
                         ? $"data:image/jpeg;base64,{Convert.ToBase64String(user.ExternalLogins.First(el => el.Provider == "Google").ProfilePhotoBytes!)}"
                         : string.Empty,
            };
            
            if (photo is null)
                return Result<UserProfileDto>.NotFound();
            return Result<UserProfileDto>.Success(photo);
                            
        }
    }
}


                            
