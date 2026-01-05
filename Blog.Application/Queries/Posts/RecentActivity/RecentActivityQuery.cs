using Blog.Application.Queries.GetRecentActivity;
using BlogApi.Domain.Common;
using MediatR;
using System.Collections.Generic;

namespace Application.Queries.GetRecentActivity
{
    public class RecentActivityQuery : IRequest<Result<List<RecentActivityItemDto>>>
    {
        public int Limit { get; set; } = 4;
        public int DaysBack { get; set; } = 7;
    }
}