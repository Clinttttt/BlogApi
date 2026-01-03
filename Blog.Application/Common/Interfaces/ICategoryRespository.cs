using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Common.Interfaces
{
    public interface ICategoryRespository
    {
        Task<List<Category>> GetListing(Expression<Func<Category, bool>>? filter = null,
        CancellationToken cancellationToken = default);
    }
}
