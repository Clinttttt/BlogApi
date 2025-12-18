using BlogApi.Application.Commands.Posts.UpdateComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class UpdateCommentRequest
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;

        public UpdateCommentCommand UpdateCommand(Guid UserId)
            => new(Id, Comment,UserId);
    }
}
