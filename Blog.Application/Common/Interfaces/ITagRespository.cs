using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Common.Interfaces
{
    public interface ITagRespository
    {
        Task<List<Tag>> GetListing(Expression<Func<Tag, bool>>? filter = null,
            CancellationToken cancellationToken = default);
    }
}
