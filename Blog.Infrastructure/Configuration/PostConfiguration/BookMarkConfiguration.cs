
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
    public class BookMarkConfiguration : IEntityTypeConfiguration<BookMark>
    {
        public void Configure(EntityTypeBuilder<BookMark> builder)
        {
            builder.HasOne(s => s.Post)
                   .WithMany(s => s.BookMarks)
                   .HasForeignKey(s => s.PostId)
                   .OnDelete(DeleteBehavior.Cascade);             
        }
    }
}
