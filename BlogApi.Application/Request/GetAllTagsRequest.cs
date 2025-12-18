using BlogApi.Application.Queries.GetAllTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class GetAllTagsRequest
    {

        public GetAllTagsQuery GetAllTagRequest(Guid UserId)
            => new(UserId);
    }
}
