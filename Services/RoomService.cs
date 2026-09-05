using ABP_test.Data;
using ABP_test.DTOs.Rooms;
using ABP_test.Models;
using ABP_test.Services.Interfaces;

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

    // Maps a Room entity to RoomResponseDto
    private static RoomResponseDto MapToDto(Room room) => new()
    {
        Id             = room.Id,
        Name           = room.Name,
        Capacity       = room.Capacity,
        BaseHourlyRate = room.BaseHourlyRate
    };
}
