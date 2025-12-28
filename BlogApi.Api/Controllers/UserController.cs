using BlogApi.Api.Shared;
using BlogApi.Application.Commands.Newsletter.SendNewsletter;
using BlogApi.Application.Commands.Newsletter.SubscribeToNewsletter;
using BlogApi.Application.Commands.Newsletter.UnSubscribeToNewsletter;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.Posts.GetUserInfo;
using BlogApi.Application.Queries.User.CurrentUser;
using BlogApi.Application.Request.Newsletter;
using BlogApi.Application.Request.User;
using BlogApi.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;


namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiBaseController
    {
        private readonly IEmailService _emailService;
        public UserController(ISender sender, IEmailService emailService) : base(sender)
        {
            _emailService = emailService;
        }

        [Authorize]
        [HttpPost("AddUserInfo")]
        public async Task<ActionResult<bool>> AddUserInfo(UserInfoRequest request)
        {
            var command = request.AddUserInfoCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPatch("UpdateUserInfo")]
        public async Task<ActionResult<bool>> UpdateUserInfo(UserInfoRequest request)
        {
            var command = request.UpdateUserInfoCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserProfileDto>> GetCurrentUser()
        {
            var command = new GetCurrentUserQuery(UserIdOrNull);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpGet("GetUserInfoDto")]
        public async Task<ActionResult<UserInfoDto>> GetUserInfoDto()
        {
            var command = new GetUserInfoQuery(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpPost("SubscribeToNewsletter")]
        public async Task<ActionResult<bool>> SubscribeToNewsletter(SubscribeToNewsletterCommand command)
        {
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("unsubscribe/{token}")]
        public async Task<ActionResult<bool>> UnSubscribeToNewsletter([FromRoute] string token)
        {
            var request = new UnSubscribeToNewsletterCommand(token);
            var result = await Sender.Send(request);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPost("SendNewsletter")]
        public async Task<ActionResult<bool>> SendNewsletter([FromBody] SendNewsletterRequest request)
        {
            var subject = $"New Post: {request.PostTitle}";

            var body = $@"
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                h1 {{ color: #6366f1; }}
                .content {{ margin: 20px 0; }}
                .footer {{ margin-top: 40px; padding-top: 20px; border-top: 1px solid #ddd; color: #666; font-size: 12px; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>{request.PostTitle}</h1>
                <div class='content'>
                    {request.PostContent}  <!-- 🔥 FULL CONTENT HERE -->
                </div>
                <div class='footer'>
                    <p>Want to read this on the website? <a href='https://yourblog.com/posts/{request.PostSlug}'>Click here</a></p>
                     <p><a href='https://localhost:7147/unsubscribe/{{{{TOKEN}}}}'>Unsubscribe</a></p>
                </div>
            </div>
        </body>
        </html>
    ";

            var command = new SendNewsletterCommand(subject, body);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
    }
}
