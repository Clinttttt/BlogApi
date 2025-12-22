using BlogApi.Application.Queries.Tags.GetAllTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Tag
{
    public class GetAllTagsRequest
    {

        public GetAllTagsQuery GetAllTagRequest(Guid UserId)
            => new(UserId);
    }
}
