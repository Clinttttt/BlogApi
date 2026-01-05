using Blog.Application.Common.Interfaces;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Queries.Posts;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using MediatR;
using System.Linq.Expressions;
using static BlogApi.Domain.Enums.EntityEnum;

public class GetPagedPostsQueryHandler(IPostRespository repository, IPostFilterBuilder builder) : IRequestHandler<GetPagedPostsQuery, Result<PagedResult<PostDto>>>
{
    public async Task<Result<PagedResult<PostDto>>> Handle(GetPagedPostsQuery request, CancellationToken cancellationToken)
    {
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
    } 
}

