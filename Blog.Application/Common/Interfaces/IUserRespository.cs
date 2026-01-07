using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Common.Interfaces
{
    public interface IUserRespository
    {
        Task<List<User>> GetListing(
            Expression<Func<User, bool>> filter,
            CancellationToken cancellationToken = default);

        Task<User> Get(Expression<Func<User, bool>> filter,
            CancellationToken cancellationToken = default);
    }
}
