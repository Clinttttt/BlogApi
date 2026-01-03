using BlogApi.Application.Commands.Posts.BookMark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Posts
{
    public class AddBookMarkRequest
    {
        public int? PostId { get; set; }

        public AddBookMarkCommand ToCommand(Guid userId)
            => new(PostId, userId);
    }
}
