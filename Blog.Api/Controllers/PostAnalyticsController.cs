using Blog.Application.Queries.Posts.GetApprovalTotal;
using Blog.Application.Queries.Posts.RecentActivity;
using BlogApi.Api.Shared;
using BlogApi.Application.Dtos;
using BlogApi.Application.Models;
using BlogApi.Application.Queries.Posts.GetPublicStatistics;
using BlogApi.Application.Queries.Posts.GetStatistics;
using BlogApi.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostAnalyticsController : ApiBaseController
    {
        public PostAnalyticsController(ISender sender) : base(sender) { }

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

        [Authorize(Roles = "Admin,Author")]
        [HttpGet("GetAdminUnreadTotal")]
        public async Task<ActionResult<UnreadDto>> GetAdminUnreadTotal()
        {
            var approval = new[] { EntityEnum.Type.PostApproval, EntityEnum.Type.PostDecline };
            var request = new GetUnreadTotalQuery
            {
                UserId = UserId,              
            };
            var result = await Sender.Send(request);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpGet("GetAuthorUnreadTotal")]
        public async Task<ActionResult<UnreadDto>> GetAuthorUnreadTotal()
        {
            var request = new GetUnreadTotalQuery
            {
                UserId = UserId,
                filter = s => s.UserId == UserId
            };
            var result = await Sender.Send(request);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserProfileStats")]
        public async Task<ActionResult<UserProfileStatsDto>> GetUserProfileStats([FromQuery] int limit = 4, [FromQuery] int daysBack = 7)
        {
            var query = new GetUserProfileStatsQuery
            {
                UserId = UserId,
                Limit = limit,
                DaysBack = daysBack
            };
            var result = await Sender.Send(query);
            return Ok(result.Value);
        }
    }
}