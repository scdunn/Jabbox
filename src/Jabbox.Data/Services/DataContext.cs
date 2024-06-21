using Jabbox.Data.Configuration;
using Jabbox.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Jabbox.Data.Services
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Post> Posts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new AccountEntityTypeConfiguration().Configure(modelBuilder.Entity<Account>());
            new PostEntityTypeConfiguration().Configure(modelBuilder.Entity<Post>());

        }

    }
}