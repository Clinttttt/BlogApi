using BlogApi.Application.Commands.Auth.Logout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Request
{
    public class LogoutRequest
    {
        public LogoutCommand LogoutCommand(Guid UserId)
            => new(UserId);
    }
}
