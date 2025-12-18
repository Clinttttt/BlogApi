using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Dtos
{
    public class PostWithCommentsDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime PostCreatedAt { get; set; }  
        public string? CategoryName { get; set; }
        public List<CommentDto> Comments { get; set; } = new();
        public List<TagDto>? Tags { get; set; } 

    }
}
