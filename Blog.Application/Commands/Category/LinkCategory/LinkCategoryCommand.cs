using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Category.LinkCategory
{
    public record LinkCategoryCommand(int PostId, int CategoryId, Guid UserId) : IRequest<Result<bool>>;
    
}
