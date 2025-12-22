using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogApi.Application.Queries.Tags.GetAllTags
{
    public record GetAllTagsQuery(Guid UserId) : IRequest<Result<List<TagDto>>>;
   
}
