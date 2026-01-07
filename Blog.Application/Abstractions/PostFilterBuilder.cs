using Blog.Application.Common.Interfaces;
using BlogApi.Application.Queries.Posts;
using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace Blog.Application.Abstractions
{
    public class PostFilterBuilder : IPostFilterBuilder
    {
        public Expression<Func<Post, bool>>? BuildFilter(GetPagedPostsQuery request)
        {
            return request.FilterType switch
            {
                PostFilterType.Published =>
                    p => p.Status == Status.Published,

                PostFilterType.PublishedByUser =>
                    p => p.UserId == request.UserId && p.Status == Status.Published,

                PostFilterType.Drafts =>
                    p => p.Status == Status.Draft,

                PostFilterType.DraftsByUser =>
                    p => p.UserId == request.UserId && p.Status == Status.Draft,

                PostFilterType.Pending =>
                    p => p.Status == Status.Pending,

                PostFilterType.PendingByUser =>
                    p => p.UserId == request.UserId && p.Status == Status.Pending,

                PostFilterType.ByCategory =>
                    p => p.CategoryId == request.CategoryId,

                PostFilterType.ByTag =>
                    p => p.PostTags.Any(pt => pt.TagId == request.TagId),

                PostFilterType.BookMark =>
                   p => p.BookMarks.Any(s => s.UserId == request.UserId),

                _ => null
            };
        }
    }
}
