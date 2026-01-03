using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Category.UnlinkCategory
{
    public record UnlinkCategoryCommand(int CategoryId,int PostId, Guid UserId) : IRequest<Result<bool>>;
    
}
