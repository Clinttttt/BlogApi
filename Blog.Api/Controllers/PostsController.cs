using Application.Queries.GetRecentActivity;
using Azure.Core;
using Blog.Application.Commands.Posts.TrackPostView;
using Blog.Application.Queries.GetRecentActivity;
using BlogApi.Api.Shared;
using BlogApi.Application.Commands.Posts.ApprovePost;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Queries.Posts; 
using BlogApi.Application.Queries.Posts.GetFeatured;
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
        [HttpGet("ListPublishedForAdmin")]
        public async Task<ActionResult<PagedResult<PostDto>>> ListPublishedForAdmin(
             [FromQuery] ListPaginatedRequest request,
             CancellationToken cancellationToken)
        {
            var query = new GetPagedPostsQuery
            {
                UserId = UserId,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                FilterType = PostFilterType.PublishedByUser
            };
            var result = await Sender.Send(query, cancellationToken);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("ListPublished")]
        public async Task<ActionResult<PagedResult<PostDto>>> ListPublished(
            [FromQuery] ListPaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetPagedPostsQuery
            {
                UserId = UserIdOrNull,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                FilterType = PostFilterType.Published
            };
            var result = await Sender.Send(query, cancellationToken);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ListDraftForAdmin")]
        public async Task<ActionResult<PagedResult<PostDto>>> ListDraftForAdmin(
            [FromQuery] ListPaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetPagedPostsQuery
            {
                UserId = UserId,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                FilterType = PostFilterType.DraftsByUser
            };
            var result = await Sender.Send(query, cancellationToken);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("ListByTag/{id}")]
        public async Task<ActionResult<PagedResult<PostDto>>> ListByTag(
            [FromRoute] int id,
            [FromQuery] ListPaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetPagedPostsQuery
            {
                UserId = UserIdOrNull,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                FilterType = PostFilterType.ByTag,
                TagId = id
            };
            var result = await Sender.Send(query, cancellationToken);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("ListByCategory/{id}")]
        public async Task<ActionResult<PagedResult<PostDto>>> ListByCategory(
            [FromRoute] int id,
            [FromQuery] ListPaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetPagedPostsQuery
            {
                UserId = UserIdOrNull,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                FilterType = PostFilterType.ByCategory,
                CategoryId = id
            };
            var result = await Sender.Send(query, cancellationToken);
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
        public async Task<ActionResult<PagedResult<PostDto>>> ListBookMark([FromQuery] ListPaginatedRequest request)
        {
            var query = new GetPagedPostsQuery
            {
                UserId = UserIdOrNull,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                FilterType = PostFilterType.BookMark,         
            };           
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
        [HttpPost("TrackPostView")]
        public async Task<ActionResult<bool>> TrackPostView([FromQuery] int PostId)
        {
            var command = new TrackPostViewCommand(UserIdOrNull, PostId);
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
        [HttpGet("GetRecentActivity")]
        public async Task<ActionResult<List<RecentActivityItemDto>>> GetRecentActivity([FromQuery] int limit = 4, [FromQuery] int daysBack = 7)
        {
            var query = new RecentActivityQuery
            {
                Limit = limit,
                DaysBack = daysBack
            };
            var result = await Sender.Send(query);
            return Ok(result.Value);
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



