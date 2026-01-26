using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Common
{
    public class ValidationErrorResponse
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, string>? Errors { get; set; } 
    }
}
