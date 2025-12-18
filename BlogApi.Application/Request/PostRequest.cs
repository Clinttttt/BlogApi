using BlogApi.Application.Commands.Posts.CreatePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BlogApi.Application.Request
{
    public class PostRequest
    {

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CategoryId { get; set; }
       
        public CreatePostCommand SetAddCommand(Guid userId)
       => new(Title, Content, CategoryId, userId);
  
    }
}
