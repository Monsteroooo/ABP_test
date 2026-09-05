namespace ABP_test.DTOs.Reports;

/// <summary>Revenue summary for a single room.</summary>
public class RoomRevenueDto
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageDurationHours { get; set; }
}

/// <summary>Full revenue report for all rooms within a date range.</summary>
public class RevenueReportDto
{
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }

    /// <summary>Combined revenue across all rooms.</summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>Per-room breakdown, sorted by revenue descending.</summary>
    public List<RoomRevenueDto> Rooms { get; set; } = [];
}
