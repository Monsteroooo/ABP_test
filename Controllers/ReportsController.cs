using ABP_test.DTOs.Reports;
using ABP_test.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ABP_test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(IReportService reportService) : ControllerBase
{
    /// <summary>Returns revenue grouped by room for the given date range.</summary>
    /// <response code="200">Revenue report generated successfully.</response>
    /// <response code="400">Invalid date range.</response>
    [HttpGet("revenue")]
    [ProducesResponseType(typeof(RevenueReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRevenueReport(
        [FromQuery] DateOnly dateFrom,
        [FromQuery] DateOnly dateTo)
    {
        try
        {
            var report = await reportService.GetRevenueReportAsync(dateFrom, dateTo);
            return Ok(report);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Returns a list of bookings with optional filters by room and date range.</summary>
    /// <response code="200">Bookings list returned successfully.</response>
    [HttpGet("bookings")]
    [ProducesResponseType(typeof(IEnumerable<BookingReportItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBookingsReport([FromQuery] BookingsFilterDto filter)
    {
        var bookings = await reportService.GetBookingsReportAsync(filter);
        return Ok(bookings);
    }
}
