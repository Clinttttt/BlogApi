using BlogApi.Application.Commands.Posts.UpdatePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class UpdatePostRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public UpdatePostCommand UpdateCommand(Guid UserId)
            => new(Id, Title, Content, UserId);
    }


}
