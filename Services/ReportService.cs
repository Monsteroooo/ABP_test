using ABP_test.Data;
using ABP_test.DTOs.Reports;
using ABP_test.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ABP_test.Services;

public class ReportService(AppDbContext db) : IReportService
{
    public async Task<RevenueReportDto> GetRevenueReportAsync(DateOnly dateFrom, DateOnly dateTo)
    {
        if (dateFrom > dateTo)
            throw new ArgumentException("Дата початку повинна бути раніше дати закінчення");

        var from = dateFrom.ToDateTime(TimeOnly.MinValue);
        var to   = dateTo.ToDateTime(TimeOnly.MaxValue);

        var bookings = await db.Bookings
            .Where(b => b.StartTime >= from && b.StartTime <= to)
            .Include(b => b.Room)
            .ToListAsync();

        // Group bookings by room and calculate revenue metrics
        var roomRevenues = bookings
            .GroupBy(b => b.Room)
            .Select(g => new RoomRevenueDto
            {
                RoomId               = g.Key.Id,
                RoomName             = g.Key.Name,
                TotalBookings        = g.Count(),
                TotalRevenue         = g.Sum(b => b.TotalCost),
                AverageDurationHours = Math.Round(g.Average(b => b.DurationHours), 1)
            })
            .OrderByDescending(r => r.TotalRevenue)
            .ToList();

        return new RevenueReportDto
        {
            DateFrom     = dateFrom,
            DateTo       = dateTo,
            TotalRevenue = roomRevenues.Sum(r => r.TotalRevenue),
            Rooms        = roomRevenues
        };
    }

    public async Task<IEnumerable<BookingReportItemDto>> GetBookingsReportAsync(BookingsFilterDto filter)
    {
        var query = db.Bookings
            .Include(b => b.Room)
            .Include(b => b.BookingServices)
                .ThenInclude(bs => bs.Service)
            .AsQueryable();

        // Apply optional filters
        if (filter.RoomId.HasValue)
            query = query.Where(b => b.RoomId == filter.RoomId.Value);

        if (filter.DateFrom.HasValue)
            query = query.Where(b => b.StartTime >= filter.DateFrom.Value.ToDateTime(TimeOnly.MinValue));

        if (filter.DateTo.HasValue)
            query = query.Where(b => b.StartTime <= filter.DateTo.Value.ToDateTime(TimeOnly.MaxValue));

        var bookings = await query.OrderBy(b => b.StartTime).ToListAsync();

        return bookings.Select(b => new BookingReportItemDto
        {
            Id            = b.Id,
            RoomId        = b.Room.Id,
            RoomName      = b.Room.Name,
            StartTime     = b.StartTime,
            EndTime       = b.StartTime.AddHours((double)b.DurationHours),
            DurationHours = b.DurationHours,
            TotalCost     = b.TotalCost,
            Services      = b.BookingServices.Select(bs => bs.Service.Name).ToList()
        });
    }
}
