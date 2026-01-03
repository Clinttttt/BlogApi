using BlogApi.Api.Shared;
using BlogApi.Application.Commands.Tags.DeleteTag;
using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.Tags.GetAllTags;
using BlogApi.Application.Queries.Tags.GetListPostTag;
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


        [Authorize(Roles = "Admin,Author")]
        [HttpPost("AddTag")]
        public async Task<ActionResult<int>> Create([FromBody] AddTagRequest request)
        {
            var command = request.AddTagCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpDelete("DeleteTag/{TagId}")]
        public async Task<ActionResult<bool>> Delete([FromQuery] int TagId)
        {
            var command = new DeleteTagCommand(TagId, UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("GetListing")]
        public async Task<ActionResult<List<TagDto>>> GetListing()
        {
            var command = new GetListQuery();
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [AllowAnonymous]
        [HttpGet("GetListingPostTag")]
        public async Task<ActionResult<List<TagDto>>> GetListingPostTag()
        {
            var command = new GetListPostTagQuery();
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPost("AddTagTopost")]
        public async Task<ActionResult<bool>> AddTagsToPost([FromBody] AddTagsToPostRequest request)
        {
            var command = request.AddTagToPostCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
    }
}
