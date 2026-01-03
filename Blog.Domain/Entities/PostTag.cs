using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class PostTag
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int PostId { get; set; }
        public Post? post { get; set; }

        public int TagId { get; set; }
        public Tag? tag { get; set; }
    }
}
