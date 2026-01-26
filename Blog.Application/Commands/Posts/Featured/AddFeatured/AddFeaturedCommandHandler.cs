using Blog.Application.Abstractions;
using Blog.Application.Common;
using Blog.Application.Common.Interfaces.Utilities;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.Featured.AddFeatured
{
    public class AddFeaturedCommandHandler(IAppDbContext context, ICacheInvalidationService cacheInvalidation) : IRequestHandler<AddFeaturedCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddFeaturedCommand request,CancellationToken cancellationToken)
        {

            var featured = await context.Featureds
                .FirstOrDefaultAsync(s => s.PostId == request.PostId, cancellationToken);

            if (featured is null)
            {              
                context.Featureds.Add(new Domain.Entities.Featured
                {
                    PostId = request.PostId,
                    UserID = request.UserId
                });
            }
            else
            {
                context.Featureds.Remove(featured);
            }

            await context.SaveChangesAsync(cancellationToken);
            await cacheInvalidation.InvalidateFeaturedCacheAsync();
            await cacheInvalidation.InvalidatePostListCachesAsync();
            return Result<bool>.Success(true);
        }
    }
}
