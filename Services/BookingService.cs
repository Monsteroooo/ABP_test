using ABP_test.Data;
using ABP_test.DTOs.Bookings;
using ABP_test.Models;
using ABP_test.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ABP_test.Services;

public class BookingService(AppDbContext db, IPricingService pricingService) : IBookingService
{
    private static readonly TimeOnly Open  = new(6,  0);
    private static readonly TimeOnly Close = new(23, 0);

    public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto)
    {
        var startTime = dto.Date.ToDateTime(dto.StartTime);
        var endTime   = startTime.AddHours((double)dto.DurationHours);

        ValidateBookingParams(dto, endTime);

        var room = await db.Rooms.FindAsync(dto.RoomId)
            ?? throw new KeyNotFoundException($"Зал з ID {dto.RoomId} не знайдено");

        await EnsureNoConflictsAsync(dto.RoomId, startTime, endTime);

        var services = await GetServicesAsync(dto.ServiceIds);

        // Calculate costs
        var roomCost     = pricingService.CalculateRoomCost(room.BaseHourlyRate, startTime, dto.DurationHours);
        var servicesCost = services.Sum(s => s.Price);
        var totalCost    = roomCost + servicesCost;

        // Persist booking
        var booking = new Booking
        {
            RoomId        = dto.RoomId,
            StartTime     = startTime,
            DurationHours = dto.DurationHours,
            TotalCost     = totalCost,
            BookingServices = services
                .Select(s => new Models.BookingService { ServiceId = s.Id })
                .ToList()
        };

        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        return MapToDto(booking, room, services, roomCost, servicesCost, totalCost);
    }

    // Validates operational hours and duration step
    private static void ValidateBookingParams(CreateBookingDto dto, DateTime endTime)
    {
        if (dto.StartTime < Open || TimeOnly.FromDateTime(endTime) > Close)
            throw new ArgumentException("Бронювання можливе тільки з 06:00 до 23:00");

        if (dto.DurationHours % 0.5m != 0)
            throw new ArgumentException("Тривалість повинна бути кратною 0.5 години");
    }

    // Checks that no existing booking overlaps the requested time slot
    private async Task EnsureNoConflictsAsync(int roomId, DateTime requestedStart, DateTime requestedEnd)
    {
        var existingBookings = await db.Bookings
            .Where(b => b.RoomId == roomId)
            .ToListAsync();

        var hasConflict = existingBookings.Any(b =>
        {
            var bookingEnd = b.StartTime.AddHours((double)b.DurationHours);
            return b.StartTime < requestedEnd && bookingEnd > requestedStart;
        });

        if (hasConflict)
            throw new InvalidOperationException("Зал вже заброньований на вибраний час");
    }

    // Fetches services by IDs and validates that all exist
    private async Task<List<Service>> GetServicesAsync(List<int> serviceIds)
    {
        if (serviceIds.Count == 0) return [];

        var services = await db.Services
            .Where(s => serviceIds.Contains(s.Id))
            .ToListAsync();

        var missingIds = serviceIds.Except(services.Select(s => s.Id)).ToList();
        if (missingIds.Count > 0)
            throw new KeyNotFoundException($"Послуги з ID {string.Join(", ", missingIds)} не знайдено");

        return services;
    }

    private static BookingResponseDto MapToDto(
        Booking booking, Room room, List<Service> services,
        decimal roomCost, decimal servicesCost, decimal totalCost) => new()
    {
        Id            = booking.Id,
        RoomId        = room.Id,
        RoomName      = room.Name,
        StartTime     = booking.StartTime,
        EndTime       = booking.StartTime.AddHours((double)booking.DurationHours),
        DurationHours = booking.DurationHours,
        RoomCost      = roomCost,
        ServicesCost  = servicesCost,
        TotalCost     = totalCost,
        Services      = services.Select(s => new BookingServiceItemDto
        {
            Id    = s.Id,
            Name  = s.Name,
            Price = s.Price
        }).ToList()
    };
}
