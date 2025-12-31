using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Dtos
{
    public class AuthorDto
    {
        public string? Name { get; set; }
        public Guid UserId { get; set; }
    }
}
