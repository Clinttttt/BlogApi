using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.User.UpdateUserInfo
{
    public class UpdateUserInfoCommandHandler(IAppDbContext context) : IRequestHandler<UpdateUserInfoCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
           var user = await context.UserInfos.FirstOrDefaultAsync(s=> s.UserId == request.UserId);
            if (user is null)
            {
                var entity = new UserInfo
                {
                    UserId = request.UserId,
                    FullName = request.FullName,
                    Bio = request.Bio,
                    Photo  = request.Photo,
                    PhotoContentType = request.PhotoContentType,
                };
                context.UserInfos.Add(entity);            
            }
            else
            {
                user.FullName = request.FullName;
                user.Bio = request.Bio;
                user.Photo = request.Photo;
                user.PhotoContentType = request.PhotoContentType;
                context.UserInfos.Update(user);
            }
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
