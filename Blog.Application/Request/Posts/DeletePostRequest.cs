using BlogApi.Application.Commands.Posts.DeletePost;
using BlogApi.Application.Commands.Posts.Featured.DeleteFeatured;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class DeletePostRequest
    {
        public int PostId { get; set; }

        public DeletePostCommand ToCommand(Guid userId)
            => new(PostId, userId);

        public DeleteFeaturedCommand DeleteCommand(Guid userId)
          => new(PostId, userId);
    }
}
