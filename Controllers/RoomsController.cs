using ABP_test.DTOs.Rooms;
using ABP_test.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ABP_test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(IRoomService roomService) : ControllerBase
{
    /// <summary>Creates a new conference room.</summary>
    /// <response code="201">Room created successfully.</response>
    /// <response code="400">Invalid input data.</response>
    [HttpPost]
    [ProducesResponseType(typeof(RoomResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto dto)
    {
        var room = await roomService.CreateRoomAsync(dto);
        return CreatedAtAction(nameof(CreateRoom), new { id = room.Id }, room);
    }

    /// <summary>Updates an existing conference room. Only provided fields are changed.</summary>
    /// <response code="200">Room updated successfully.</response>
    /// <response code="400">Invalid input data.</response>
    /// <response code="404">Room not found.</response>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(RoomResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDto dto)
    {
        try
        {
            var room = await roomService.UpdateRoomAsync(id, dto);
            return Ok(room);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
