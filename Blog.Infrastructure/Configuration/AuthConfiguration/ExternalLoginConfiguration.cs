using BlogApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Configuration.AuthConfiguration
{
    public class ExternalLoginConfiguration : IEntityTypeConfiguration<ExternalLogin>
    {

        public void Configure(EntityTypeBuilder<ExternalLogin> builder)
        {

            // Each ExternalLogin belongs to one User
            // Each User can have many ExternalLogins
            // Cascade delete: if a User is deleted, all their ExternalLogins are also deleted
            builder.HasOne(el => el.User)
                .WithMany(s=> s.ExternalLogins)
                .HasForeignKey(s=> s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
