using BlogApi.Application.Commands.Comment.UpdateComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class UpdateCommentRequest
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = string.Empty;

        public UpdateCommentCommand ToCommand(Guid userId)
            => new(CommentId, Content, userId);
    }
}
