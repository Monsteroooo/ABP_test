using ABP_test.DTOs.Rooms;

namespace ABP_test.Services.Interfaces;

public interface IRoomService
{
    Task<RoomResponseDto> CreateRoomAsync(CreateRoomDto dto);
    Task<RoomResponseDto> UpdateRoomAsync(int id, UpdateRoomDto dto);
    Task DeleteRoomAsync(int id);
    Task<IEnumerable<RoomResponseDto>> SearchAvailableRoomsAsync(SearchRoomsDto dto);
}
