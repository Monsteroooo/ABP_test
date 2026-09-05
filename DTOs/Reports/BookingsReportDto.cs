namespace ABP_test.DTOs.Reports;

/// <summary>Query parameters for filtering the bookings list report.</summary>
public class BookingsFilterDto
{
    /// <summary>Filter by room ID (optional).</summary>
    public int? RoomId { get; set; }

    /// <summary>Include bookings starting on or after this date (optional).</summary>
    public DateOnly? DateFrom { get; set; }

    /// <summary>Include bookings starting on or before this date (optional).</summary>
    public DateOnly? DateTo { get; set; }
}

/// <summary>A single booking entry in the bookings list report.</summary>
public class BookingReportItemDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal DurationHours { get; set; }
    public decimal TotalCost { get; set; }
    public List<string> Services { get; set; } = [];
}
