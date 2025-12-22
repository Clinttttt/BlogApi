using BlogApi.Application.Queries.Category.GetAllCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Category
{
    public class GetAllCategoryRequest
    {
        public GetAllCategoryQuery GetAllCategoryQuery(Guid UserId)
            => new(UserId);
    }
}
