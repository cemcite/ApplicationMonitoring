using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NuevoSoftware.ApplicationMonitoring.Data
{
    public partial class NSAppMonDBContext : DbContext
    {
        public NSAppMonDBContext()
        {
        }

        public NSAppMonDBContext(DbContextOptions<NSAppMonDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NsapplicationsT> NsapplicationsT { get; set; }
        public virtual DbSet<NsresourcesT> NsresourcesT { get; set; }
        public virtual DbSet<NsusersT> NsusersT { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=NSAppMonDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NsapplicationsT>(entity =>
            {
                entity.ToTable("NSApplicationsT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NsapplicationsT)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NSApplicationsT_NSUsersT");
            });

            modelBuilder.Entity<NsresourcesT>(entity =>
            {
                entity.ToTable("NSResourcesT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ResourceKey)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ResourceText).IsRequired();
            });

            modelBuilder.Entity<NsusersT>(entity =>
            {
                entity.ToTable("NSUsersT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
