using BlogApi.Api.Shared;
using BlogApi.Application.Commands.Tags.DeleteTag;
using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.Tags.GetAllTags;
using BlogApi.Application.Request.Tag;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ApiBaseController
    {
        public TagController(ISender sender) : base(sender) { }


        [Authorize]
        [HttpPost("AddTag")]
        public async Task<ActionResult<int>> Create([FromBody] AddTagRequest request)
        {
            var command = request.AddTagCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize]
        [HttpDelete("DeleteTag/{TagId}")]
        public async Task<ActionResult<bool>> Delete([FromQuery] int TagId)
        {
            var command = new DeleteTagCommand(TagId, UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpGet("GetAllTags")]
        public async Task<ActionResult<List<TagDto>>> GetAllTags()
        {
            var command = new GetAllTagsQuery(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPost("AddTagTopost")]
        public async Task<ActionResult<bool>> AddTagsToPost([FromBody] AddTagsToPostRequest request)
        {
            var command = request.AddTagToPostCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
    }
}
