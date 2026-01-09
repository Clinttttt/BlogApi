using Blog.Application.Common.Interfaces;
using Blog.Infrastructure.Services;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Queries.Posts;
using BlogApi.Domain.Common;
using MediatR;

namespace BlogApi.Application.Queries.Posts.Handlers;

public class GetPagedPostsQueryHandler(IPostRespository repository,IPostFilterBuilder builder, ICacheService cacheService) : IRequestHandler<GetPagedPostsQuery, Result<PagedResult<PostDto>>>
{
    public async Task<Result<PagedResult<PostDto>>> Handle(GetPagedPostsQuery request,CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetPagedPosts(
            request.UserId,
            request.PageNumber,
            request.PageSize,
            request.FilterType.ToString(),
            request.TagId,
            request.CategoryId);

       
        var expiration = GetCacheExpiration(request.FilterType);
       
        var result = await cacheService.GetOrCreateAsync(cacheKey,
            async () =>
            {
                var filter = builder.BuildFilter(request);
                var postPage = await repository.GetPaginatedPostDtoAsync(
                    request.UserId,
                    request.PageNumber,
                    request.PageSize,
                    filter: filter,
                    cancellationToken);

                return postPage.Value;
            },
            expiration,
            cancellationToken);

        if (result == null || !result.Items.Any())
            return Result<PagedResult<PostDto>>.NoContent();

        return Result<PagedResult<PostDto>>.Success(result);
    }
    private static TimeSpan GetCacheExpiration(PostFilterType filterType)
    {
        return filterType switch
        {
            PostFilterType.Published => CacheKeys.Expiration.Medium,
            PostFilterType.ByTag => CacheKeys.Expiration.Medium,
            PostFilterType.ByCategory => CacheKeys.Expiration.Medium,
            PostFilterType.PublishedByUser => CacheKeys.Expiration.Short,
            PostFilterType.DraftsByUser => CacheKeys.Expiration.Short,
            PostFilterType.Pending => CacheKeys.Expiration.Short,
            PostFilterType.BookMark => CacheKeys.Expiration.Short,
            _ => CacheKeys.Expiration.Short
        };
    }
}