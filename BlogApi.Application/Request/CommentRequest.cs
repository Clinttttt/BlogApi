using BlogApi.Application.Commands.Posts.AddComment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class CommentRequest
    {
        public int Id { get; set; } 
        public string Content { get; set; } = string.Empty;

        public AddCommentCommand SetAddCommand(Guid UserId)
            => new(Id, Content, UserId);
    }
}
