using BlogApi.Application.Commands.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class AddCategoyRequest
    {
        public string? Name { get; set; }
      public AddCategoyCommand AddCategoyCommand(Guid UserId)
            =>new (Name, UserId);

    }
}
