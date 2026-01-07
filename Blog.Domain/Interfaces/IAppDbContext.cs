using Blog.Domain.Entities;
using BlogApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Post> Posts { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<PostLike> PostLikes { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<PostTag> PostTags { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<CommentLike> CommentLikes { get; set; }
        DbSet<BookMark> BookMarks { get; set; }
        DbSet<ExternalLogin> ExternalLogins { get; set; }
        DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        DbSet<UserInfo> UserInfos { get; set; }
        DbSet<Featured> Featureds { get; set; }
        DbSet<PostView> PostViews { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
