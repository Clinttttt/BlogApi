using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Abstractions
{
    public static class CacheTracker
    {
        public static HashSet<string> Keys { get; } = new();
    }
}
