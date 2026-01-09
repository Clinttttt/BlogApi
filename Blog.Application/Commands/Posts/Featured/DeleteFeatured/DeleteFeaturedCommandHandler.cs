using Blog.Application.Abstractions;
using Blog.Application.Common;
using Blog.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.Featured.DeleteFeatured
{
    public class DeleteFeaturedCommandHandler(IAppDbContext context, ICacheInvalidationService cacheInvalidation) : IRequestHandler<DeleteFeaturedCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(
            DeleteFeaturedCommand request,
            CancellationToken cancellationToken)
        {         
            var featured = await context.Featureds
                .FirstOrDefaultAsync(
                    s => s.PostId == request.PostId && s.UserID == request.UserId,
                    cancellationToken);

            if (featured is null)
                return Result<bool>.NotFound();

       
            context.Featureds.Remove(featured);
            await context.SaveChangesAsync(cancellationToken);
            await cacheInvalidation.InvalidateFeaturedCacheAsync();
            return Result<bool>.Success(true);
        }
    }
}
