using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Dtos
{
    public class PostDetailDto
    {
        public int? PostId { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime PostCreatedAt { get; set; }  
        public string? CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public List<CommentDto> Comments { get; set; } = new();
        public List<TagDto>? Tags { get; set; } 
        public byte[]? Photo { get; set; }
        public string? PhotoContent { get; set; }
        public string? Author { get; set; }
        public int? ViewCount { get; set; }
        public ReadingDuration readingDuration { get; set; }
        public int PostLike { get; set; }
        public int CommentCount { get; set; }
        public int BookMarkCount { get; set; }
        public bool PhotoIsliked { get; set; }
        public bool IsBookMark { get; set; }
        public Status Status { get; set; }

    }
}
