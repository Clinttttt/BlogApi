using BlogApi.Api.Shared;
using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.GetAllTags;
using BlogApi.Application.Request;
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
        [HttpGet("AddTag")]
        public async Task<ActionResult<TagDto>> Create([FromQuery] AddTagRequest request)
        {
            var command = request.AddTagCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize]
        [HttpDelete("DeleteTag")]
        public async Task<ActionResult<bool>> Delete([FromQuery] DeleteTagRequest request)
        {
            var command = request.DeleteTagCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpGet("GetAllTags")]
        public async Task<ActionResult<List<TagDto>>> GetAllTagsQueryHandler([FromQuery] GetAllTagsRequest request)
        {
            var command = request.GetAllTagRequest(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
    }
}
