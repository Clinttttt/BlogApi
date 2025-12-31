using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Posts.ApprovePost
{
    public record ApprovePostCommand(int PostId) : IRequest<Result<bool>>;
    
}
