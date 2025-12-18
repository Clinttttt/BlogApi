using BlogApi.Application.Queries.GetAllCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class GetAllCategoryRequest
    {
        public GetAllCategoryQuery GetAllCategoryQuery(Guid UserId)
            => new(UserId);
    }
}
