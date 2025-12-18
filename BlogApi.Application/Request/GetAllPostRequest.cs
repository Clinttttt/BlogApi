using BlogApi.Application.Queries.GetAllPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class GetAllPostRequest
    {
        public string Query { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public GetAllPostsQuery GetAllPostCommand(Guid UserId) 
            => new(CategoryId, UserId, Query);  
    }
}
