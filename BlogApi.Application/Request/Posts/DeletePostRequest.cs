using BlogApi.Application.Commands.Posts.DeletePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class DeletePostRequest
    {
        public int Id { get; set; }

        public DeletePostCommand DeleteCommand(Guid UserId)
            => new(Id, UserId);
    }
}
