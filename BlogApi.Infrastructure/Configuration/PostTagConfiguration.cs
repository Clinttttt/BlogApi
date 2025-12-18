using BlogApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Configuration
{
    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> Builder)
        {
            Builder.HasKey(pt => new { pt.PostId, pt.TagId });

            Builder.HasOne(s => s.post)
                .WithMany(s => s.PostTags)
                .HasForeignKey(s => s.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            Builder.HasOne(s => s.tag)
                .WithMany(s => s.PostTags)
                .HasForeignKey(s => s.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
