using BlogApi.Api.Shared;
using BlogApi.Application.Dtos;

using BlogApi.Application.Models;
using BlogApi.Application.Queries.Posts.GetPostByTag;
using BlogApi.Application.Queries.Posts.GetPostPaged;
using BlogApi.Application.Queries.Posts.GetPostWithComments;
using BlogApi.Application.Queries.Posts.GetRecentPost;
using BlogApi.Application.Request.Posts;
using BlogApi.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ApiBaseController
    {
        public PostsController(ISender sender) : base(sender) { }

        [Authorize]
        [HttpPost("CreatePosts")]
        public async Task<ActionResult<int>> Create([FromBody] PostRequest request)
        {
            var command = request.SetAddCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("GetPost")]
        public async Task<ActionResult<PostWithCommentsDto>> Get([FromQuery] GetPostWithCommentsQuery Id)
        {
            var request = await Sender.Send(Id);
            return HandleResponse(request);

        }
        [Authorize]
        [HttpDelete("DeletePost")]
        public async Task<ActionResult<bool>> Delete([FromQuery] DeletePostRequest request)
        {
            var command = request.DeleteCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPatch("UpdatePost")]
        public async Task<ActionResult<int>> Update([FromBody] UpdatePostRequest request)
        {
            var command = request.UpdateCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPatch("ArchivedPost")]
        public async Task<ActionResult<bool>> Archived([FromQuery] ArchivedRequest request)
        {
            var command = request.ArchivedCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpGet("GetAllPost")]
        public async Task<ActionResult<List<PostDto>>> GetListing([FromQuery] GetAllPostRequest request)
        {
            var command = request.GetAllPostCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPost("CreateComment")]
        public async Task<ActionResult<int>> CreateComments([FromBody] CommentRequest request)
        {
            var command = request.SetAddCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPatch("UpdateComment")]
        public async Task<ActionResult<int>> UpdateComment([FromBody] UpdateCommentRequest request)
        {
            var command = request.UpdateCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("GetPostPaged")]
        public async Task<ActionResult<PagedResult<PostDto>>> GetPostPaged([FromQuery] GetPostPagedQuery command)
        {
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("GetRecentPost")]
        public async Task<ActionResult<List<PostDto>>> GetRecentPost([FromQuery] GetRecentPostQuery command)
        {
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPost("PostLike")]
        public async Task<ActionResult<bool>> PostLike([FromQuery] TogglePostLikeRequest request)
        {
            var command = request.ToggleCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
     
        [AllowAnonymous]
        [HttpPost("GetPostByTag")]
        public async Task<ActionResult<List<PostDto>>> GetPostByTag([FromQuery] GetPostByTagQuery command)
        {         
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

    }
}
