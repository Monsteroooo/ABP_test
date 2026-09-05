namespace ABP_test.Models;

/// <summary>
/// Junction entity that links a booking to its selected services.
/// </summary>
public class BookingService
{
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
}
