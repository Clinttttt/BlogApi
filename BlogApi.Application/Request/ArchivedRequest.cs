using BlogApi.Application.Commands.Posts.ArchivePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class ArchivedRequest
    {
        public int Id { get; set; }
        public ArchivePostCommand ArchivedCommand(Guid UserId)
            => new(Id, UserId);
    }
}
