using ABP_test.Data;
using ABP_test.DTOs.Rooms;
using ABP_test.Models;
using ABP_test.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ABP_test.Services;

public class RoomService(AppDbContext db) : IRoomService
{
    public async Task<RoomResponseDto> CreateRoomAsync(CreateRoomDto dto)
    {
        var room = new Room
        {
            Name            = dto.Name,
            Capacity        = dto.Capacity,
            BaseHourlyRate  = dto.BaseHourlyRate
        };

        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        return MapToDto(room);
    }

    public async Task<RoomResponseDto> UpdateRoomAsync(int id, UpdateRoomDto dto)
    {
        var room = await db.Rooms.FindAsync(id)
            ?? throw new KeyNotFoundException($"Зал з ID {id} не знайдено");

        // Update only fields that were provided (not null)
        if (dto.Name           is not null) room.Name           = dto.Name;
        if (dto.Capacity       is not null) room.Capacity       = dto.Capacity.Value;
        if (dto.BaseHourlyRate is not null) room.BaseHourlyRate = dto.BaseHourlyRate.Value;

        await db.SaveChangesAsync();

        return MapToDto(room);
    }

    public async Task DeleteRoomAsync(int id)
    {
        var room = await db.Rooms.FindAsync(id)
            ?? throw new KeyNotFoundException($"Зал з ID {id} не знайдено");

        // Prevent deletion if the room has existing bookings
        var hasBookings = await db.Bookings.AnyAsync(b => b.RoomId == id);
        if (hasBookings)
            throw new InvalidOperationException($"Неможливо видалити зал з ID {id}: існують пов'язані бронювання");

        db.Rooms.Remove(room);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<RoomResponseDto>> SearchAvailableRoomsAsync(SearchRoomsDto dto)
    {
        ValidateSearchParams(dto);

        var requestedStart = dto.Date.ToDateTime(dto.StartTime);
        var requestedEnd   = dto.Date.ToDateTime(dto.EndTime);

        // Load rooms with sufficient capacity and their bookings
        var rooms = await db.Rooms
            .Where(r => r.Capacity >= dto.Capacity)
            .Include(r => r.Bookings)
            .ToListAsync();

        // Filter in memory: exclude rooms where any booking overlaps the requested slot
        return rooms
            .Where(r => !r.Bookings.Any(b =>
            {
                var bookingEnd = b.StartTime.AddHours((double)b.DurationHours);
                // Overlap: booking starts before requested end AND booking ends after requested start
                return b.StartTime < requestedEnd && bookingEnd > requestedStart;
            }))
            .Select(MapToDto);
    }

    // Validates that the search time range is within operational hours (06:00–23:00)
    private static void ValidateSearchParams(SearchRoomsDto dto)
    {
        var open  = new TimeOnly(6,  0);
        var close = new TimeOnly(23, 0);

        if (dto.StartTime < open || dto.EndTime > close)
            throw new ArgumentException("Зали працюють з 06:00 до 23:00");

        if (dto.StartTime >= dto.EndTime)
            throw new ArgumentException("Час початку повинен бути раніше часу закінчення");
    }

    // Maps a Room entity to RoomResponseDto
    private static RoomResponseDto MapToDto(Room room) => new()
    {
        Id             = room.Id,
        Name           = room.Name,
        Capacity       = room.Capacity,
        BaseHourlyRate = room.BaseHourlyRate
    };
}
