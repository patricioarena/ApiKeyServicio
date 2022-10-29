using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Models
{
    public partial class ApiKeyDbContext : DbContext
    {
        public ApiKeyDbContext()
        {
        }

        public ApiKeyDbContext(DbContextOptions<ApiKeyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Key> Keys { get; set; }
        public virtual DbSet<Key_Application> Key_Applications { get; set; }
        public virtual DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=devserver04;Database=interfaz_entrada;Trusted_Connection=True;App=EntityFramework;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application", "ApiKey");

                entity.Property(e => e.description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "ApiKey");

                entity.Property(e => e.client1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("client");

                entity.Property(e => e.created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dischargeDate).HasColumnType("datetime");

                entity.Property(e => e.emiter_user)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(suser_sname())");

                entity.Property(e => e.revoke_user)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Key>(entity =>
            {
                entity.ToTable("Key", "ApiKey");

                entity.Property(e => e.apiKey).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dischargeDate).HasColumnType("datetime");

                entity.Property(e => e.emiter_user)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(suser_sname())");

                entity.Property(e => e.ipEnd)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ipStart)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.revoke_user)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.client)
                    .WithMany(p => p.Keys)
                    .HasForeignKey(d => d.clientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Key_Client");
            });

            modelBuilder.Entity<Key_Application>(entity =>
            {
                entity.ToTable("Key_Application", "ApiKey");

                entity.HasIndex(e => new { e.clientId, e.applicationId, e.keyId }, "uq_Key_Application")
                    .IsUnique();

                entity.Property(e => e.dischargeDate).HasColumnType("datetime");

                entity.Property(e => e.emiter_user)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(suser_sname())");

                entity.Property(e => e.enabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.enabledDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.revoke_user)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.application)
                    .WithMany(p => p.Key_Applications)
                    .HasForeignKey(d => d.applicationId)
                    .HasConstraintName("FK_Key_Application_Application");

                entity.HasOne(d => d.key)
                    .WithMany(p => p.Key_Applications)
                    .HasForeignKey(d => d.keyId)
                    .HasConstraintName("FK_Key_Application_Key");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log", "ApiKey");

                entity.Property(e => e.description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.incidentDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.remoteIp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.application)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.applicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Log_Application");

                entity.HasOne(d => d.client)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.clientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Log_Client");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
