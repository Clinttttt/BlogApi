using BlogApi.Api.Shared;
using BlogApi.Application.Commands.Auth.Login;
using BlogApi.Application.Commands.Auth.RefreshToken;
using BlogApi.Application.Commands.Auth.Register;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Request.Auth;
using BlogApi.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiBaseController
    {
        public AuthController(ISender sender) : base(sender) { }

        [HttpPost("Register")]
        public async Task<ActionResult<AuthResult>> Register([FromBody] RegisterCommand command)
        {
            var register = await Sender.Send(command);        
            return HandleResponse(register);
        }
    
        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] AuthRequest command)
        {
            var request = command.ToLoginCommand();
            var login = await Sender.Send(request);        
            return HandleResponse(login);
        }
        [HttpPost("GoogleLogin")]
        public async Task<ActionResult<TokenResponseDto>> GoogleLogin([FromBody] GoogleLoginCommand command)
        {
            var login = await Sender.Send(command);
            return HandleResponse(login);
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<ActionResult<bool>> Logout([FromQuery] LogoutRequest request)
        {
            var command = request.LogoutCommand(UserId);
            var login = await Sender.Send(command);
            return HandleResponse(login);
        }
       
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenCommand command)
        {
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
    }
}
