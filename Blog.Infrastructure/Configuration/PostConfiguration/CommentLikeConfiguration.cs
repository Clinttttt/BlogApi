using BlogApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Configuration.PostConfiguration
{
    public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
    {
        public void Configure(EntityTypeBuilder<CommentLike> builder)
        {

            builder.HasOne(s => s.Comments)
                .WithMany(s => s.CommentLikes)
                .HasForeignKey(s => s.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
