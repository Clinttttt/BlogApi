using Blog.Application.Abstractions;
using Blog.Application.Common;
using Blog.Application.Common.Interfaces;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Queries.Posts;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.BookMark
{
    public class AddBookMarkCommandHandler(IAppDbContext context, ICacheInvalidationService cacheInvalidation) : IRequestHandler<AddBookMarkCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddBookMarkCommand request,CancellationToken cancellationToken)
        {
      
            var bookmark = await context.BookMarks.FirstOrDefaultAsync(s => s.PostId == request.PostId && s.UserId == request.UserId,cancellationToken);
            if (bookmark is null)
            {             
                context.BookMarks.Add(new Domain.Entities.BookMark
                {
                    PostId = request.PostId,
                    CreatedAt = DateTime.UtcNow.AddHours(8),
                    UserId = request.UserId
                });
            }
            else
            {           
                context.BookMarks.Remove(bookmark);
            }
   
            await context.SaveChangesAsync(cancellationToken);
            await cacheInvalidation.InvalidatePostListCachesAsync();  
            await cacheInvalidation.InvalidatePostCacheAsync(request.PostId);
            return Result<bool>.Success(true);
        }
    }
}
