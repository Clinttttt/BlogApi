using BlogApi.Application.Commands.Category.LinkCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Category
{
    public class LinkCategoryRequest
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }
        public LinkCategoryCommand linkCategoryCommand(Guid UserId)
            => new(PostId, CategoryId, UserId);
    }
}
