using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BiluthyrningAB.Models.Entities
{
    public partial class MercuryContext : DbContext
    {
        public MercuryContext()
        {
        }

        public MercuryContext(DbContextOptions<MercuryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<HistoryLog> HistoryLog { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(" Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog= Mercury;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cars>(entity =>
            {
                entity.Property(e => e.CarType).HasMaxLength(32);

                entity.Property(e => e.RegNr).HasMaxLength(6);
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Ssn)
                    .HasColumnName("SSN")
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HistoryLog>(entity =>
            {
                entity.HasOne(d => d.Car)
                    .WithMany(p => p.HistoryLog)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__HistoryLo__CarId__2E1BDC42");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.HistoryLog)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__HistoryLo__Custo__2D27B809");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.HistoryLog)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__HistoryLo__Order__2C3393D0");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.BookingNr);

                entity.Property(e => e.BookingNr).HasColumnName("BookingNR");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PickUpDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__Orders__CarId__286302EC");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Orders__Customer__276EDEB3");
            });
        }
    }
}
