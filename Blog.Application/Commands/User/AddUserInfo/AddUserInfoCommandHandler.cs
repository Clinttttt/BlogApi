using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.User.AddUserInfo
{
    public class AddUserInfoCommandHandler(IAppDbContext context) : IRequestHandler<AddUserInfoCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddUserInfoCommand request, CancellationToken cancellationToken)
        {
            var user = await context.UserInfos.FirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (user is null)
                context.UserInfos.Add(new Domain.Entities.UserInfo
                {
                    Bio = request.Bio,
                    FullName = request.FullName,
                    UserId = request.UserId,
                    Photo = request.Photo,
                    PhotoContentType = request.PhotoContentType,
                });
            else
                return Result<bool>.Conflict();
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
