using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Post Post { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
    }
}
