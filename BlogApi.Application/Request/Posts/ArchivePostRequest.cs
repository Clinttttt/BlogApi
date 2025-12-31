using BlogApi.Application.Commands.Posts.ArchivePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class ArchivePostRequest
    {
        public int PostId { get; set; }

        public ArchivePostCommand ToCommand(Guid userId)
            => new(PostId, userId);
    }
}
