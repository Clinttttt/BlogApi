using BlogApi.Application.Commands.Category.DeleteCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Category
{
    public class DeleteCategoryRequest
    {
        public int CategoryId { get; set; }
        public DeleteCategoryCommand deleteCategoryCommand(Guid UserId)
            => new(CategoryId, UserId);
    }
}
