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

    // Maps a Room entity to RoomResponseDto
    private static RoomResponseDto MapToDto(Room room) => new()
    {
        Id             = room.Id,
        Name           = room.Name,
        Capacity       = room.Capacity,
        BaseHourlyRate = room.BaseHourlyRate
    };
}
