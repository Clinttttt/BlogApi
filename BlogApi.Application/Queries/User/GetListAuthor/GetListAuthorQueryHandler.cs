using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Queries.User.GetListAuthor
{
    public class GetListAuthorQueryHandler(IUserRespository  respository) : IRequestHandler<GetListAuthorQuery, Result<List<AuthorDto>>>
    {
        public async Task<Result<List<AuthorDto>>> Handle(GetListAuthorQuery request, CancellationToken cancellationToken)
        {
            var user = await respository.GetListing(
                filter: s => s.Role == "Author" && s.Posts != null, cancellationToken);

            if (!user.Any())
                return Result<List<AuthorDto>>.NoContent();

            var filter = user.Select(s => new AuthorDto
            {
                Name = s.UserName,
                UserId = s.Id,
            }).ToList();

            return Result<List<AuthorDto>>.Success(filter);
        }
    }
}
