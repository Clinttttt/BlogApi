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
                Status = request.Status,
                readingDuration = request.readingDuration

            };

            foreach (var tagid in request.TagIds.Distinct())
            {
                post.PostTags.Add(new PostTag
                {
                    TagId = tagid,
                    UserId = request.UserId

                });
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return Result<int>.Success(post.Id);
        }
    }
}
