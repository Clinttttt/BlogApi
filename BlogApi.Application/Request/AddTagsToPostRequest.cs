using BlogApi.Application.Commands.Posts.AddTagsToPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class AddTagsToPostRequest
    {
        public int PostId { get; set; }
        public int Tagid { get; set; }
        public AddTagsToPostCommand AddTagToPostCommand(Guid UserId)
            => new(PostId, Tagid, UserId);
    }
}
