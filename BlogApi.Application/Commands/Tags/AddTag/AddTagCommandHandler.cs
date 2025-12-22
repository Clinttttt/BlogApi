using AutoMapper;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IMapper = AutoMapper.IMapper;
namespace BlogApi.Application.Commands.Tags.AddTag
{
    public class AddTagCommandHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<AddTagCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddTagCommand request, CancellationToken cancellationToken)
        {
            if (await context.Tags.AnyAsync(s => s.Name == request.Name))
            {
                return Result<bool>.Conflict();
            }
            var tag = new Tag
            {
                Name = request.Name,
                UserId = request.UserId,
            };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();
            var tagdto = mapper.Map<bool>(tag);
            return Result<bool>.Success(tagdto);

        }
    }
}
