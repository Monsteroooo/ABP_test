using ABP_test.DTOs.Reports;

namespace ABP_test.Services.Interfaces;

public interface IReportService
{
    /// <summary>Returns revenue grouped by room for the given date range.</summary>
    Task<RevenueReportDto> GetRevenueReportAsync(DateOnly dateFrom, DateOnly dateTo);

    /// <summary>Returns a filtered list of all bookings.</summary>
    Task<IEnumerable<BookingReportItemDto>> GetBookingsReportAsync(BookingsFilterDto filter);
}
