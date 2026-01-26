using BlogApi.Domain.Common;
using MediatR;
using System.Collections.Generic;

namespace Blog.Application.Queries.Posts.RecentActivity
{
    public class GetUserProfileStatsQuery : IRequest<Result<UserProfileStatsDto>>
    {
        public Guid UserId { get; set; }
        public int Limit { get; set; } = 4;
        public int DaysBack { get; set; } = 7;
    }
}