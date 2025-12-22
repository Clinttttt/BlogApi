using BlogApi.Api.Shared;
using BlogApi.Application.Commands.Category.DeleteCategory;
using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.Category.GetAllCategory;
using BlogApi.Application.Request.Category;
using BlogApi.Domain.Entities;
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
        public async Task<ActionResult<bool>> Create([FromQuery] AddCategoryRequest request)
        {
            var command = request.AddCategoyCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<List<CategoryDto>>> GetListing()
        {
            var query = new GetAllCategoryQuery(UserId);
            var result = await Sender.Send(query);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<ActionResult<bool>> Delete([FromQuery] int Id)
        {
            var command = new DeleteCategoryCommand(Id, UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPost("UnlinkCategory")]
        public async Task<ActionResult<bool>> Unlink([FromBody] UnlinkCategoryRequest request)
        {
            var command = request.UnlinkCategoryCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize]
        [HttpPost("LinkCategory")]
        public async Task<ActionResult<bool>> link([FromBody] LinkCategoryRequest request)
        {
            var command = request.linkCategoryCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
    }
}
