using BlogApi.Application.Commands.Comment.AddComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{

    public class AddCommentRequest
    {
        public int PostId { get; set; }
        public string Content { get; set; } = string.Empty;

        public AddCommentCommand ToCommand(Guid userId)
            => new(PostId, Content, userId);
    }
}
