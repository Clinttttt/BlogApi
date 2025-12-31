using BlogApi.Application.Commands.Posts.Featured.AddFeatured;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class AddFeaturedRequest
    {
        public int PostId { get; set; }

        public AddFeaturedCommand ToCommand(Guid userId)
            => new(PostId, userId);
    }
}
