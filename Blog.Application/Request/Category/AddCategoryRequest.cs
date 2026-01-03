using BlogApi.Application.Commands.Category.AddCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Category
{
    public class AddCategoryRequest
    {
        public string? Name { get; set; }
      public AddCategoyCommand AddCategoyCommand(Guid UserId)
            =>new (Name, UserId);

    }
}
