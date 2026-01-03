using BlogApi.Api.Shared;
using BlogApi.Application.Commands.Posts.ApprovePost;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Queries.BookMark.GetAllBookMark;
using BlogApi.Application.Queries.Posts.GetByCategory;
using BlogApi.Application.Queries.Posts.GetFeatured;
using BlogApi.Application.Queries.Posts.GetListByTag;
using BlogApi.Application.Queries.Posts.GetPublicStatistics;
using BlogApi.Application.Queries.Posts.GetStatistics;
using BlogApi.Application.Request.Posts;
using BlogApi.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ApiBaseController
    {
        public PostsController(ISender sender) : base(sender) { }

      

         [Authorize(Roles = "Admin,Author")]
        [HttpPost("AddPost")]
        public async Task<ActionResult<int>> AddPost([FromBody] CreatePostRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("GetPost")]
        public async Task<ActionResult<PostDetailDto>> GetPost([FromQuery] GetPostRequest request)
        {
            var query = request.ToQuery(UserIdOrNull);
            var result = await Sender.Send(query);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPatch("UpdatePost")]
        public async Task<ActionResult<int>> UpdatePost([FromBody] UpdatePostRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpDelete("DeletePost")]
        public async Task<ActionResult<bool>> DeletePost([FromQuery] DeletePostRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPatch("ArchivedPost")]
        public async Task<ActionResult<bool>> ArchivedPost([FromQuery] ArchivePostRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }




        [Authorize(Roles = "Admin")]
        [HttpGet("ListForAdmin")]
        public async Task<ActionResult<PagedResult<PostDto>>> ListForAdmin(
             [FromQuery] ListForAdminPostsRequest request,
             CancellationToken cancellationToken)
        {
            var query = request.ToQuery(UserId);
            var result = await Sender.Send(query, cancellationToken);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("ListPublished")]
        public async Task<ActionResult<PagedResult<PostDto>>> ListPublished(
            [FromQuery] ListPublishedPostsRequest request,
            CancellationToken cancellationToken)
        {
            var query = request.ToQuery();
            var result = await Sender.Send(query, cancellationToken);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("ListByTag/{id}")]
        public async Task<ActionResult<List<PostDto>>> ListByTag([FromRoute] int id)
        {
            var query = new GetListByTagQuery(id);
            var result = await Sender.Send(query);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("ListByCategory/{id}")]
        public async Task<ActionResult<List<PostDto>>> ListByCategory([FromRoute] int id)
        {
            var query = new GetListByCategoryQuery(id);
            var result = await Sender.Send(query);
            return HandleResponse(result);
        }





        [Authorize(Roles = "Admin,Author")]
        [HttpPost("ToggleLikePost")]
        public async Task<ActionResult<bool>> ToggleLikePost([FromQuery] TogglePostLikeRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPost("AddBookMark")]
        public async Task<ActionResult<bool>> AddBookMark([FromQuery] AddBookMarkRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpGet("ListBookMark")]
        public async Task<ActionResult<List<PostDto>>> ListBookMark()
        {
            var query = new GetListQuery(UserId);
            var result = await Sender.Send(query);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddFeatured")]
        public async Task<ActionResult<bool>> AddFeatured([FromBody] AddFeaturedRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteFeatured")]
        public async Task<ActionResult<bool>> DeleteFeatured([FromBody] DeletePostRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("ListFeatured")]
        public async Task<ActionResult<List<FeaturedPostDto>>> ListFeatured()
        {
            var query = new GetListFeaturedQuery();
            var result = await Sender.Send(query);
            return HandleResponse(result);
        }





        [Authorize(Roles = "Admin,Author")]
        [HttpPost("AddComment")]
        public async Task<ActionResult<int>> AddComment([FromBody] AddCommentRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPatch("UpdateComment")]
        public async Task<ActionResult<int>> UpdateComment([FromBody] UpdateCommentRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPost("ToggleLikeComment")]
        public async Task<ActionResult<bool>> ToggleLikeComment([FromBody] ToggleCommentLikeRequest request)
        {
            var command = request.ToCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }




        [AllowAnonymous]
        [HttpGet("GetPublicStatistics")]
        public async Task<ActionResult<StatisticsDto>> GetPublicStatistics()
        {
            var command = new GetPublicStatisticsQuery();
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetStatistics")]
        public async Task<ActionResult<StatisticsDto>> GetStatistics()
        {
            var command = new GetStatisticsQuery(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ApprovePost")]
        public async Task<ActionResult<bool>> ApprovePost([FromQuery] ApprovePostCommand command)
        {
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

    }
}