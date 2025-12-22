using BlogApi.Application.Commands.Tags.AddTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request.Tag
{
    public class AddTagRequest
    {
        public string? Name { get; set; }
        public AddTagCommand AddTagCommand(Guid UserId)
            => new(Name, UserId);
    }
}
