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
                .AsNoTracking()
                .Include(s=> s.Posts)
                .ToListAsync();
        }
        public async Task<User> Get(Expression<Func<User,bool>> filter, CancellationToken cancellationToken = default)
        {
             IQueryable<User>user = context.Users.AsNoTracking();

            if (filter != null)
                user = user.Where(filter);

            return await user
                .AsNoTracking()
                .Include(s => s.Posts)
                .Include(s=> s.UserInfo)
                .Include(s=> s.ExternalLogins)
                .FirstOrDefaultAsync();
        }
        
    }
}
