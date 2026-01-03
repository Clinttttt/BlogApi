using AutoMapper;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.LikePost
{
    public class TogglePostLikeCommandHandler(IAppDbContext context) : IRequestHandler<TogglePostLikeCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(TogglePostLikeCommand request, CancellationToken cancellationToken)
        {
            var like = await context.PostLikes
                 .FirstOrDefaultAsync(s => s.PostId == request.PostId && s.UserId == request.UserId);
            
            if (like is null)
            {
                context.PostLikes.Add(new PostLike
                {
                    PostId = request.PostId,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow.AddHours(8)    
                });
            }
            else
            {
                context.PostLikes.Remove(like);
            }
            await context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}
