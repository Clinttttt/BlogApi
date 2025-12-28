using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;


namespace BlogApi.Application.Dtos
{
    public class PostWithStatusDto : PostDto
    {
        public Status Status { get; set; }
    }
}
