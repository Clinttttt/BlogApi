using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public Guid UserId { get; set; } 
        public string? Name { get; set; }
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
