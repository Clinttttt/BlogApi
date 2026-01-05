using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Slug { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
