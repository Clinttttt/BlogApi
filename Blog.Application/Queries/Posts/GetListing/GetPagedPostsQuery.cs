using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using MediatR;
using System.Linq.Expressions;

namespace BlogApi.Application.Queries.Posts
{

    public record GetPagedPostsQuery : IRequest<Result<PagedResult<PostDto>>>
    {
        public Guid? UserId { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public PostFilterType FilterType { get; init; }


        public int? CategoryId { get; init; }
        public int? TagId { get; init; }
    }

    public enum PostFilterType
    {
        BookMark,
        Published,              // All published posts
        PublishedByUser,        // Published posts by specific user
        Drafts,                 // All drafts
        DraftsByUser,          // Drafts by specific user
        Pending,               // Pending approval
        PendingByUser,         // Pending by specific user
        ByCategory,            // Filter by category
        ByTag                  // Filter by tag
    }
}