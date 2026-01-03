using BlogApi.Application.Abstractions;
using BlogApi.Application.Queries.Posts.GetListForAdmin;
using BlogApi.Application.Queries.Posts.GetPostPaged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class ListForAdminPostsRequest : PageRequest
    {
        public GetListForAdminQuery ToQuery(Guid UserId)
          => new(UserId ,PageNumber, PageSize);
    }
}
