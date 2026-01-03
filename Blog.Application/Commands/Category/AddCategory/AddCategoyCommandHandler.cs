using BlogApi.Application.Commands.Category.AddCategory;
using BlogApi.Domain.Common;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Commands.Category
{
    public class AddCategoyCommandHandler(IAppDbContext context) : IRequestHandler<AddCategoyCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddCategoyCommand request, CancellationToken cancellationToken)
        {
           var category = await context.Categories.AnyAsync(s=> s.Name == request.Name);
            if (category)
            {
                return Result<bool>.Conflict();
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                var slughelper = SlugHelper.Generate(request.Name);
                var categ = new BlogApi.Domain.Entities.Category
                {
                    Name = request.Name,
                    Slug = slughelper,
                    UserId = request.UserId
                };
                context.Categories.Add(categ);
            }
                 
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
