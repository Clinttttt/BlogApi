using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class Featured
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Guid UserID { get; set; }
        public Post? Post { get; set; }
    }
}
