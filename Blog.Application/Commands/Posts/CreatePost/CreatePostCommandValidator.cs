using BlogApi.Application.Commands.Posts.CreatePost;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Commands.Posts.CreatePost
{
    public class CreatePostCommandValidator :  AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(s => s.Title)
                    .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("Title is required")
                   .MaximumLength(50).WithMessage("Title must not exceed 50 characters")
                   .Matches(@"^[\w#\- ]+$").WithMessage("Tag name can contain letters, numbers, spaces, #, - and _");
           
            RuleFor(s => s.Content)
                   .Cascade(CascadeMode.Stop)
                  .NotEmpty().WithMessage("Content is required")
                  .Must(content => !string.IsNullOrWhiteSpace(System.Text.RegularExpressions.Regex.Replace(content ?? "", "<.*?>", "")))
                  .WithMessage("Content is required");

            RuleFor(s => s.Author)
               .Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage("Author is required");
        }

    }
}
