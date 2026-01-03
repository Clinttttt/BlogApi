
using BlogApi.Application.Commands.Category.UnlinkCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Category
{
    public class UnlinkCategoryRequest
    {
        public int CategoryId { get; set; }
        public int PostId { get; set; }
        public UnlinkCategoryCommand UnlinkCategoryCommand(Guid UserId)
            => new(CategoryId, PostId, UserId);
    }
}
