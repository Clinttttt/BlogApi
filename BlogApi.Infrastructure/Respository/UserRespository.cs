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
    public class UserRespository(IAppDbContext context) : IUserRespository
    {
        public async Task<List<User>> GetListing(Expression<Func<User,bool>> filter, CancellationToken cancellationToken = default)
        {
            IQueryable<User> user = context.Users.AsNoTracking();

            if (filter != null)
                user = user.Where(filter);

            return await user
                .Include(s=> s.Posts)
                .ToListAsync();
        }
    }
}
