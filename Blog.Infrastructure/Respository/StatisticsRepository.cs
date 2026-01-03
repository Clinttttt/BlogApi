using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlogApi.Infrastructure.Respository
{
    public class StatisticsRepository(IAppDbContext context) : IStatisticsRepository
    {
        public async Task<StatisticsDto> GetStatisticsAsync(Expression<Func<Post, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
              IQueryable<Post> query = context.Posts.AsNoTracking();
            
            if (filter != null)
                query = query.Where(filter);

            var posts = await query.ToListAsync(cancellationToken);

            return new StatisticsDto
            {
                TotalPosts = posts.Count(),
                DraftPosts = posts.Count( s=> s.Status == Status.Draft),
                PublishedPosts = posts.Count(s=> s.Status == Status.Published),
            };
        }
    }
}
