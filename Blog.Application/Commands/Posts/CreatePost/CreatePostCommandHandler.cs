using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace BlogApi.Application.Commands.Posts.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result<int>>
    {
        private readonly IAppDbContext _context;
        public CreatePostCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(s => s.Id == request.UserId, cancellationToken);

                if (user is null)
                    return Result<int>.NotFound();

                if (user.Role != "Author" && user.Role != "Admin")
                    return Result<int>.Failure();

                var postStatus = user.Role == "Author"
                    ? Status.Pending
                    : request.Status;

                var post = new Post
                {
                    Title = request.Title,
                    Content = request.Content,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow.AddHours(8),
                    CategoryId = request.CategoryId,
                    Photo = request.Photo,
                    PhotoContent = request.PhotoContent,
                    Author = request.Author,
                    Status = postStatus,
                    readingDuration = request.readingDuration

                };

                foreach (var tagId in request.TagIds.Distinct())
                {
                    post.PostTags.Add(new PostTag
                    {
                        TagId = tagId,
                        UserId = request.UserId

                    });
                }

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return Result<int>.Success(post.Id);
            }
        }
    }
}
