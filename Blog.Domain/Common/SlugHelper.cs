using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Common
{
    public  static class SlugHelper
    {
        public static string Generate(string text)
        {
            return text
            .ToLower()
            .Trim()
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace(",", "");
        }
    }
}
