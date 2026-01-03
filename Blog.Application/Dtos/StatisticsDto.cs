using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Dtos
{
    public class StatisticsDto
    {
        public int TotalPosts { get; set; }
        public int DraftPosts { get; set; }
        public int PublishedPosts { get; set; }
    }
}
