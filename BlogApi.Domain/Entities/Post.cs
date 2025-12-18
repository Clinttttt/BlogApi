using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get;  set; }
        public string? Content { get;  set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get;  set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
    public enum Status
    {
        Active,
        Archived
    }
}
