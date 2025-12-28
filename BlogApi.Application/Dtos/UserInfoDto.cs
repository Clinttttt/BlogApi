using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Dtos
{
    public class UserInfoDto
    {
        public string? FullName { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
    }


}
