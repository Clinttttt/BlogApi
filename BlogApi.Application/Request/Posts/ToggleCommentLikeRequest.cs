using BlogApi.Application.Commands.Comment.LikeComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class ToggleCommentLikeRequest
    {
        public int CommentId { get; set; }
     
        public ToggleCommentLikeCommand ToggleCommentLikeCommand(Guid UserId)
            => new(CommentId, UserId);
    }
}
