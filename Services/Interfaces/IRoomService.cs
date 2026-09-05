using ABP_test.DTOs.Rooms;

namespace ABP_test.Services.Interfaces;

public interface IRoomService
{
    Task<RoomResponseDto> CreateRoomAsync(CreateRoomDto dto);
}
