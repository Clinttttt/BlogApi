using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class CommentLike
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int CommentId { get; set; }
        public Comment? Comments { get; set; }
    }
}
