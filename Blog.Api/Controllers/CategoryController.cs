using BlogApi.Api.Shared;
using BlogApi.Application.Commands.Category.DeleteCategory;
using BlogApi.Application.Dtos;
using BlogApi.Application.Queries.Category.GetAllCategory;
using BlogApi.Application.Queries.Category.GetListPostCategory;
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


        [Authorize(Roles = "Admin,Author")]
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<bool>> Create([FromBody] AddCategoryRequest request)
        {
            var command = request.AddCategoyCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("GetListing")]
        public async Task<ActionResult<List<CategoryDto>>> GetListing()
        {
            var query = new GetListQuery();
            var result = await Sender.Send(query);
            return HandleResponse(result);
        }
        [AllowAnonymous]
        [HttpGet("GetListPostCategory")]
        public async Task<ActionResult<List<CategoryDto>>> GetListPostCategory()
        {
            var query = new GetListPostCategoryQuery();
            var result = await Sender.Send(query);
            return HandleResponse(result);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpDelete("DeleteCategory/{Id}")]
        public async Task<ActionResult<bool>> Delete([FromQuery] int Id)
        {
            var command = new DeleteCategoryCommand(Id, UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize(Roles = "Admin,Author")]
        [HttpPost("UnlinkCategory")]
        public async Task<ActionResult<bool>> Unlink([FromBody] UnlinkCategoryRequest request)
        {
            var command = request.UnlinkCategoryCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
        [Authorize(Roles = "Admin,Author")]
        [HttpPost("LinkCategory")]
        public async Task<ActionResult<bool>> link([FromBody] LinkCategoryRequest request)
        {
            var command = request.linkCategoryCommand(UserId);
            var result = await Sender.Send(command);
            return HandleResponse(result);
        }
    }
}
