using Microsoft.EntityFrameworkCore;
using ApiKey.Models;

namespace ApiKey.Data
{
    public class WhitelistDbContext : DbContext
    {
        public DbSet<WhitelistEntry> WhitelistEntries { get; set; }

        public WhitelistDbContext(DbContextOptions<WhitelistDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WhitelistEntry>()
                .HasIndex(e => e.Value)
                .IsUnique();
        }
    }
}
