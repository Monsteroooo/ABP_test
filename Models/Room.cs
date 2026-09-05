namespace ABP_test.Models;

/// <summary>
/// Represents a conference room available for booking.
/// </summary>
public class Room
{
    public int Id { get; set; }

    /// <summary>Room display name (e.g. "Зал A").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Maximum number of people the room can accommodate.</summary>
    public int Capacity { get; set; }

    /// <summary>Base rental price per hour in UAH.</summary>
    public decimal BaseHourlyRate { get; set; }

    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
