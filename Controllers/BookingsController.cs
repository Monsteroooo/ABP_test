using ABP_test.DTOs.Bookings;
using ABP_test.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ABP_test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    /// <summary>Creates a new room booking with cost calculation.</summary>
    /// <response code="201">Booking created. Returns confirmation with total cost.</response>
    /// <response code="400">Invalid input data or booking time outside operational hours.</response>
    /// <response code="404">Room or service not found.</response>
    /// <response code="409">Room is already booked for the selected time.</response>
    [HttpPost]
    [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
    {
        try
        {
            var booking = await bookingService.CreateBookingAsync(dto);
            return CreatedAtAction(nameof(CreateBooking), new { id = booking.Id }, booking);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
