using Blog.Application.Common.Interfaces;
using BlogApi.Application.Common.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace Blog.Application.Queries.Posts.GetApprovalTotal
{
    public class GetApprovalTotalQueryHandler(
        IAppDbContext context) 
        : IRequestHandler<GetApprovalTotalQuery, Result<GetApprovalTotalDto>>
    {
        private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(5);

        public async Task<Result<GetApprovalTotalDto>> Handle(
            GetApprovalTotalQuery request,
            CancellationToken cancellationToken)
        {

          

             
            var postCount = await context.Posts
                .AsNoTracking()
                .Where(s => s.Status == Status.Pending)
                .CountAsync(cancellationToken);

            var dto = new GetApprovalTotalDto { Count = postCount };

            
            if (dto.Count <= 0)
            {
                var noContent = Result<GetApprovalTotalDto>.NoContent();
               
                return noContent;
            }
    
            var result = Result<GetApprovalTotalDto>.Success(dto);
            

            return result;
        }
    }
}
