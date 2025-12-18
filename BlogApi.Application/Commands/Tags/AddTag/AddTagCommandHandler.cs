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
    public class AddTagCommandHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<AddTagCommand, Result<TagDto>>
    {
        public async Task<Result<TagDto>> Handle(AddTagCommand request, CancellationToken cancellationToken)
        {
            if (await context.Tags.AnyAsync(s => s.Name == request.Name))
            {
                return Result<TagDto>.Conflict();
            }
            var tag = new Tag
            {
                Name = request.Name,
                UserId = request.UserId,
            };
            context.Tags.Add(tag);
            await context.SaveChangesAsync();
            var tagdto = mapper.Map<TagDto>(tag);
            return Result<TagDto>.Success(tagdto);

        }
    }
}
