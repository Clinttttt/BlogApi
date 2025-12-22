using BlogApi.Application.Commands.Tags.DeleteTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Tag
{
    public class DeleteTagRequest
    {
        public int TagId { get; set; }
        public DeleteTagCommand DeleteTagCommand(Guid UserId)
            => new(TagId, UserId);
    }
}
