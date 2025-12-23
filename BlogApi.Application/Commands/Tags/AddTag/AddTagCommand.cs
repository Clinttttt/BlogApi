using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Tags.AddTag
{
    public record AddTagCommand(string? Name, Guid UserId) : IRequest<Result<int>>;
   
}
