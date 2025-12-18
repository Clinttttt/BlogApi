using BlogApi.Api.Shared;
using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.GetRecentPost;
using BlogApi.Application.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ApiBaseController
    {
        public CategoryController(ISender sender) : base(sender) { }


        [Authorize]
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<bool>> Create([FromQuery] AddCategoyRequest request)
        {
            var command = request.AddCategoyCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

        [Authorize]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<List<CategoryDto>>> GetListing([FromQuery] GetAllCategoryRequest request)
        {
            var command = request.GetAllCategoryQuery(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }

    }
}
