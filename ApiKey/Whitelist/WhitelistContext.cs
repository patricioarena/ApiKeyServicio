using Microsoft.EntityFrameworkCore;
using ApiKey.Whitelist;

namespace ApiKey.Data
{
    public class WhitelistContext : DbContext
    {
        public DbSet<EntryEntity> WhitelistEntries { get; set; }

        public WhitelistContext(DbContextOptions<WhitelistContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntryEntity>()
                .HasIndex(e => e.Value)
                .IsUnique();
        }
    }
}
