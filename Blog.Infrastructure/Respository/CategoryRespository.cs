using BlogApi.Application.Common.Interfaces;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Respository
{
    public class CategoryRespository(IAppDbContext context) : ICategoryRespository
    {
        public async Task<List<Category>> GetListing(Expression<Func<Category, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> query = context.Categories.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            return await query         
                .Include(s=> s.Posts)
                .ToListAsync(cancellationToken);

        }
    }
}
