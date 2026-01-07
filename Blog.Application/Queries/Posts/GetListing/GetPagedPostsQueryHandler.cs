using Blog.Application.Common.Interfaces;

using BlogApi.Application.Common.Interfaces;

using BlogApi.Application.Dtos;

using BlogApi.Application.Models;

using BlogApi.Application.Queries.Posts;

using BlogApi.Domain.Common;

using BlogApi.Domain.Entities;

using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;

using Microsoft.Extensions.Caching.Memory;

using System.Linq.Expressions;

using static BlogApi.Domain.Enums.EntityEnum;

public class GetPagedPostsQueryHandler(IPostRespository repository, IPostFilterBuilder builder, IMemoryCache cache) : IRequestHandler<GetPagedPostsQuery, Result<PagedResult<PostDto>>>

{

    public async Task<Result<PagedResult<PostDto>>> Handle(GetPagedPostsQuery request, CancellationToken cancellationToken)

    {

        var cacheKey = $"paginated-post-{request.FilterType}-{request.UserId}-{request.CategoryId}-{request.TagId}-{request.PageNumber}-{request.PageSize}";

        if (cache.TryGetValue(cacheKey, out Result<PagedResult<PostDto>>? cachedValue))

            return cachedValue;

        return await cache.GetOrCreateAsync(cacheKey, async entry =>

        {

            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

            var filter = builder.BuildFilter(request);
            var postPage = await repository.GetPaginatedPostDtoAsync(
                request.UserId,
                request.PageNumber,
                request.PageSize,
                filter: filter,
                cancellationToken);

            if (!postPage.Value!.Items.Any())

                return Result<PagedResult<PostDto>>.NoContent();

            return Result<PagedResult<PostDto>>.Success(postPage.Value);

        })!;



    }

}

