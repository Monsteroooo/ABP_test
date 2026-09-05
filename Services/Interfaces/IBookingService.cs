using ABP_test.DTOs.Bookings;

namespace ABP_test.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto);
}
