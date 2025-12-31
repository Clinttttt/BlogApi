using BlogApi.Application.Commands.Posts.UpdatePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Request.Posts
{
    public class UpdatePostRequest
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public byte[]? Photo { get; set; }
        public string? PhotoContent { get; set; }
        public string? Author { get; set; }
        public ReadingDuration ReadingDuration { get; set; }

        public UpdatePostCommand ToCommand(Guid userId)
            => new(PostId, Title, Content, Photo, PhotoContent, Author, ReadingDuration, userId);
    }
}
