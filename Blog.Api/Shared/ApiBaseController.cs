using BlogApi.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApi.Api.Shared
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {

        private readonly ISender _sender;

        protected ApiBaseController(ISender sender)
        {
            _sender = sender;
        }
        protected ISender Sender => _sender;

        /*   protected Guid UserId
           {
               get
               {
                   var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                  return Guid.TryParse(userId, out var Id)
                         ? Id : throw new UnauthorizedAccessException("Invalid or missing user ID in token.");
                   return Guid.TryParse(userId, out var id) ? id : null;
               }
   }
   */
        protected Guid? UserIdOrNull
        {
            get
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(userId, out var id) ? id : null;
            }
        }
        protected Guid UserId
        {
            get
            {
                var id = UserIdOrNull;
                if (id == null)
                    throw new UnauthorizedAccessException("User is not authenticated.");
                return id.Value;
            }
        }

        protected ActionResult<T> HandleResponse<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            return result.StatusCode switch
            {
                404 => NotFound(),
                401 => Unauthorized(),
                403 => Forbid(),
                409 => Conflict(),
                204 => NoContent(),     
                500 => StatusCode(500, "Internal Server Error"),
                _ => BadRequest()
            };
        }
    
    }
}
