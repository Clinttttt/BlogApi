using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Dtos
{
    public class TagDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? TagCount { get; set; }
        public int? PostId { get; set; }
      
    }
}
