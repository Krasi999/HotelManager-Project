using HotelManager.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManager.Data
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Number).IsRequired().HasMaxLength(10);
                entity.Property(r => r.Type).IsRequired().HasMaxLength(50);
                entity.Property(r => r.PricePerNight).HasColumnType("decimal(10,2)");
                entity.Property(r => r.Description).HasMaxLength(500);
            });

            modelBuilder.Entity<Guest>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(g => g.LastName).IsRequired().HasMaxLength(100);
                entity.Property(g => g.Email).HasMaxLength(200);
                entity.Property(g => g.Phone).HasMaxLength(20);
                entity.Property(g => g.EGN).HasMaxLength(10);

                entity.Ignore(g => g.FullName);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Status).IsRequired().HasMaxLength(20);
                entity.Property(r => r.TotalPrice).HasColumnType("decimal(10,2)");

                entity.HasOne(r => r.Room)
                      .WithMany()
                      .HasForeignKey(r => r.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Guest)
                      .WithMany()
                      .HasForeignKey(r => r.GuestId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Ignore(r => r.Nights);
            });
        }
    }
}