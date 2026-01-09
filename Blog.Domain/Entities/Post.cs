using Blog.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get;  set; }
        public string? Content { get;  set; }
        public byte[]? Photo { get; set; }
        public string? PhotoContent { get; set; }
        public int? ViewCount { get; set; }
        public string? Author { get; set; }
        public Status Status { get; set; } 
        public DateTime CreatedAt { get;  set; }
        public ReadingDuration readingDuration { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public ICollection<BookMark> BookMarks { get; set; } = new List<BookMark>();
        public ICollection<Featured> Featured { get; set; } = new List<Featured>();
        public ICollection<PostView> PostViews { get; set; } = new List<PostView>();
    }



}
