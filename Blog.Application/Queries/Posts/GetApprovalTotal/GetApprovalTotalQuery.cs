using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Queries.Posts.GetApprovalTotal
{
    public class GetApprovalTotalQuery() : IRequest<Result<GetApprovalTotalDto>>;
   
}
