using BlogApi.Api.Shared;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Queries.Posts.GetAllBookMark;
using BlogApi.Application.Queries.Posts.GetFeatured;
using BlogApi.Application.Queries.Posts.GetPostByTag;
using BlogApi.Application.Queries.Posts.GetPostPaged;
using BlogApi.Application.Queries.Posts.GetPostWithComments;
using BlogApi.Application.Queries.Posts.GetRecentPost;
using BlogApi.Application.Queries.Posts.PostDashboard;
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
        public async Task<ActionResult<PostWithCommentsDto>> Get([FromQuery] GetPostWithCommentsRequest request)
        {
            var command = request.GetPostWithCommentsQuery(UserIdOrNull);
            var result = await Sender.Send(command);
            return HandleResponse(result);

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
        public async Task<ActionResult<List<PostDto>>> GetRecentPost()
        {
            var command = new GetRecentPostQuery();
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
        [Authorize]
        [HttpPost("CommentLike")]
        public async Task<ActionResult<bool>> CommentLike([FromBody] ToggleCommentLikeRequest request)
        {
            var command = request.ToggleCommentLikeCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPost("AddBookMark")]
        public async Task<ActionResult<bool>> BookMark(AddBookMarkRequest request)
        {
            var command = request.AddBookMarkCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpGet("GetBookMark")]
        public async Task<ActionResult<List<PostDto>>> GetBookMark()
        {
            var command = new GetAllBookMarkQuery(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpGet("PostDashboard")]
        public async Task<ActionResult<PostDashboardDto>> PostDashboard()
        {
            var command = new PostDashboardQuery(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPost("AddFeatured")]
        public async Task<ActionResult<bool>> AddFeatured([FromBody] AddFeaturedRequest request)
        {
            var command = request.AddFeaturedCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("GetFeatured")]
        public async Task<ActionResult<FeaturedPostDto>> GetFeatured()
        {
            var command = new GetFeaturedQuery();
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
    }
}
