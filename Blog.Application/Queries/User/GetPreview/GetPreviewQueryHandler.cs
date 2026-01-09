using Blog.Application.Common.Interfaces;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.User.CurrentUser;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.User.GetCurrentUser
{
    public class GetPreviewQueryHandler(
        IUserRespository repository
       ) 
        : IRequestHandler<GetPreviewQuery, Result<UserProfileDto>>
    {
        private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(10);

        public async Task<Result<UserProfileDto>> Handle(
            GetPreviewQuery request,
            CancellationToken cancellationToken)
        {
         
          
                      
            var user = await repository.Get(filter: s => s.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                var noContent = Result<UserProfileDto>.NotFound();
              
                return noContent;
            }
         
            var dto = new UserProfileDto
            {
                PhotoUrl = user.UserInfo?.Photo != null && user.UserInfo.Photo.Length > 0
                    ? $"data:{user.UserInfo.PhotoContentType};base64,{Convert.ToBase64String(user.UserInfo.Photo)}"
                    : user.ExternalLogins
                        .FirstOrDefault(el => el.Provider == "Google" && el.ProfilePhotoBytes != null) != null
                        ? $"data:image/jpeg;base64,{Convert.ToBase64String(user.ExternalLogins.First(el => el.Provider == "Google").ProfilePhotoBytes!)}"
                        : string.Empty,
                Name = user.UserInfo?.FullName ?? user.UserName,
            };

            var result = Result<UserProfileDto>.Success(dto);     

            return result;
        }
    }
}
