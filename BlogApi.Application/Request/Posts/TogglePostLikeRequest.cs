using BlogApi.Application.Commands.Posts.LikePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class TogglePostLikeRequest
    {
        public int PostId { get; set; }
        public TogglePostLikeCommand ToggleCommand(Guid UserId)
            => new(PostId, UserId);
    }
}
