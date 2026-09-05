namespace ABP_test.Models;

/// <summary>
/// Represents a conference room booking made by a client.
/// </summary>
public class Booking
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    /// <summary>Date and time when the booking starts.</summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Duration of the booking in hours. Must be a multiple of 0.5 (e.g. 1, 1.5, 2).
    /// </summary>
    public decimal DurationHours { get; set; }

    /// <summary>Total cost of the booking including room rate and selected services in UAH.</summary>
    public decimal TotalCost { get; set; }

    // Navigation properties
    public ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}
