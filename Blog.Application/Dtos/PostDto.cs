using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Dtos
{
    public class PostDto 
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CategoryName { get; set; }
        public ReadingDuration readingDuration { get; set; }
        public List<TagDto>? Tags { get; set; }
        public bool IsBookMark { get; set; }
        public Status Status { get; set; }
    }

}
