namespace ABP_test.Models;

/// <summary>
/// Represents an additional service that can be added to a booking (e.g. projector, Wi-Fi).
/// </summary>
public class Service
{
    public int Id { get; set; }

    /// <summary>Service name (e.g. "Проєктор").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Fixed price of the service in UAH.</summary>
    public decimal Price { get; set; }

    // Navigation properties
    public ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}
