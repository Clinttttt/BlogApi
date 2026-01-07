using Blog.Domain.Common;
using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class PostView : BaseEntity
    {
        public int PostId { get; set; }
        public Guid? UserId { get; set; }
        public Post? Post { get; set; }
    }
}
