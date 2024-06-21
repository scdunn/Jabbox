using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Jabbox.Data.Models;
using System.Reflection.Emit;

namespace Jabbox.Data.Configuration
{
    public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // Posts
            builder
                .Property(b => b.Message)
                .HasColumnType("nvarchar(max)");
            builder
                .Property(b => b.PostedDate)
                .HasColumnType("datetime");
        }
    }
}
