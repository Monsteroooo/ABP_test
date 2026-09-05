namespace ABP_test.DTOs.Bookings;

/// <summary>
/// A single service included in a booking.
/// </summary>
public class BookingServiceItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

/// <summary>
/// Booking confirmation returned to the client with full cost breakdown.
/// </summary>
public class BookingResponseDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal DurationHours { get; set; }

    /// <summary>Cost of room rental only (with tariff applied).</summary>
    public decimal RoomCost { get; set; }

    /// <summary>Total cost of all selected services.</summary>
    public decimal ServicesCost { get; set; }

    /// <summary>Total cost = room cost + services cost.</summary>
    public decimal TotalCost { get; set; }

    public List<BookingServiceItemDto> Services { get; set; } = [];
}
