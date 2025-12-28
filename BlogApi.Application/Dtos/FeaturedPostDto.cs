using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Dtos
{
    public class FeaturedPostDto
    {
        public string? Title { get; set; } 
        public string? Content { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReadingDuration ReadingDuration { get; set; }
    }
}
