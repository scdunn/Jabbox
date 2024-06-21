using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Jabbox.Data.Models;

namespace Jabbox.Data.Configuration
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder
                .HasMany(c => c.Posts)
                .WithOne(e => e.Account)
                .IsRequired();

            builder
                .Property(p => p.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(p => p.PasswordHash)
                .HasMaxLength(256)
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(p => p.PasswordSalt)
                .HasMaxLength(256)
                .IsUnicode(false)
                .IsRequired();
        }
    }
}
