using Blog.Application.Abstractions;
using Blog.Application.Common;
using Blog.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.DeletePost
{
    public class DeletePostCommandHandler(IAppDbContext context, ICacheInvalidationService cacheInvalidation) : IRequestHandler<DeletePostCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(
            DeletePostCommand request,
            CancellationToken cancellationToken)
        {          
            var post = await context.Posts
                .FirstOrDefaultAsync(
                    s => s.Id == request.Id && s.UserId == request.UserId,
                    cancellationToken);

            if (post is null)
                return Result<bool>.NotFound();

            context.Posts.Remove(post);
            await context.SaveChangesAsync(cancellationToken);
            await cacheInvalidation.InvalidatePostListCachesAsync();
            await cacheInvalidation.InvalidateTagsCacheAsync();
            await cacheInvalidation.InvalidateCategoryCacheAsync();
            return Result<bool>.Success(true);
        }
    }
}
