using BlogApi.Application.Queries.Posts.GetAllPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class GetAllPostRequest
    {
        public string Query { get; set; } = string.Empty;     
        public GetAllPostsQuery GetAllPostCommand(Guid UserId) 
            => new( UserId, Query);  
    }
}
