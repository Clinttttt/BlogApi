using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Models
{
    public class AuthResult
    {     
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
