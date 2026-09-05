using ABP_test.Models;
using Microsoft.EntityFrameworkCore;

namespace ABP_test.Data;

/// <summary>
/// Main database context for the conference room booking application.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingService> BookingServices => Set<BookingService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite primary key for junction table (BookingId + ServiceId)
        modelBuilder.Entity<BookingService>()
            .HasKey(bs => new { bs.BookingId, bs.ServiceId });

        // Booking → Room (many-to-one)
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId);

        // BookingService → Booking (many-to-one)
        modelBuilder.Entity<BookingService>()
            .HasOne(bs => bs.Booking)
            .WithMany(b => b.BookingServices)
            .HasForeignKey(bs => bs.BookingId);

        // BookingService → Service (many-to-one)
        modelBuilder.Entity<BookingService>()
            .HasOne(bs => bs.Service)
            .WithMany(s => s.BookingServices)
            .HasForeignKey(bs => bs.ServiceId);

        // Decimal precision for money fields
        modelBuilder.Entity<Room>()
            .Property(r => r.BaseHourlyRate)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<Service>()
            .Property(s => s.Price)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalCost)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<Booking>()
            .Property(b => b.DurationHours)
            .HasColumnType("decimal(4,1)");

        SeedData(modelBuilder);
    }

    /// <summary>
    /// Seeds initial rooms and services as specified in the task requirements.
    /// </summary>
    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, Name = "Зал A", Capacity = 50,  BaseHourlyRate = 2000 },
            new Room { Id = 2, Name = "Зал B", Capacity = 100, BaseHourlyRate = 3500 },
            new Room { Id = 3, Name = "Зал C", Capacity = 30,  BaseHourlyRate = 1500 }
        );

        modelBuilder.Entity<Service>().HasData(
            new Service { Id = 1, Name = "Проєктор", Price = 500 },
            new Service { Id = 2, Name = "Wi-Fi",    Price = 300 },
            new Service { Id = 3, Name = "Звук",     Price = 700 }
        );
    }
}
