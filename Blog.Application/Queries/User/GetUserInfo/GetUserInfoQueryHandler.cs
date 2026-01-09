using Blog.Application.Common.Interfaces;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.User.GetUserInfo
{
    public class GetUserInfoQueryHandler(
        IUserRespository repository
   ) 
        : IRequestHandler<GetUserInfoQuery, Result<UserInfoDto>>
    {
        private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(10);

        public async Task<Result<UserInfoDto>> Handle(
            GetUserInfoQuery request,
            CancellationToken cancellationToken)
        {
            
           
           
            var user = await repository.Get(
                filter: s => s.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (user is null)
            {
                var noContent = Result<UserInfoDto>.NoContent();
             
                return noContent;
            }

          
            var dto = new UserInfoDto
            {
                FullName = user.UserInfo?.FullName ?? user.UserName,
                ProfilePhoto = user.UserInfo?.Photo != null && user.UserInfo.Photo.Length > 0
                    ? $"data:{user.UserInfo.PhotoContentType};base64,{Convert.ToBase64String(user.UserInfo.Photo)}"
                    : user.ExternalLogins.Select(x => x.ProfilePhotoUrl).FirstOrDefault(),
                Bio = user.UserInfo?.Bio ?? "Add a short bio",
                CreatedAt = user.ExternalLogins.Select(s => s.LinkedAt).FirstOrDefault(),
            };

            var result = Result<UserInfoDto>.Success(dto);

         
           

            return result;
        }
    }
}
