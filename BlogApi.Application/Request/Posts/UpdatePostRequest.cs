using BlogApi.Application.Commands.Posts.UpdatePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enum.EntityEnum;

namespace BlogApi.Application.Request.Posts
{
    public class UpdatePostRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public byte[]? Photo { get; set; }
        public string? PhotoContent { get; set; }
        public string? Author { get; set; }
        public ReadingDuration readingDuration { get; set; }



        public UpdatePostCommand UpdateCommand(Guid UserId)
            => new(Id, Title, Content, Photo, PhotoContent, Author, readingDuration, UserId);
    }


}
