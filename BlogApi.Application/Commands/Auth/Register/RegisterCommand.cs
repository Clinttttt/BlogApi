using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Auth.Register
{
    public record RegisterCommand(string username,string password) : IRequest<Result<AuthResult>>;
    
}
