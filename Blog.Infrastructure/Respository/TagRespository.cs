using BlogApi.Application.Common.Interfaces;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Respository
{
    public class TagRespository(IAppDbContext context) : ITagRespository
    {
        public async Task<List<Tag>> GetListing(Expression<Func<Tag, bool>>? filter = null,CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> query = context.Tags.AsNoTracking();

            if(filter is not null)
                query = query.Where(filter);

            return await query
                .Include(s=> s.PostTags)
                .ToListAsync(cancellationToken);
          
        }
    }
}
