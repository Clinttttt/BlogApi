using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Entities
{
    public class BookMark
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int? PostId { get; set; }
        public Post Post { get; set; } = null!;
  
    }
}
