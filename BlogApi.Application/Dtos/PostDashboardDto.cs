using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Dtos
{
    public class PostDashboardDto
    {
        public int TotalPosts { get; set; }
        public int DraftPosts { get; set; }
        public int PublishedPosts { get; set; }
        public List<PostWithStatusDto> Posts { get; set; } = new List<PostWithStatusDto>();
    }
}
