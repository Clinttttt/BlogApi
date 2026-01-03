using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Dtos
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserName { get; set; }
        public string? PhotoUrl { get; set; }
        public int LikeCount { get; set; }
        public bool IsLikedComment { get; set; }
 

    }
}
