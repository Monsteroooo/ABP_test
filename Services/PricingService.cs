using ABP_test.Services.Interfaces;

namespace ABP_test.Services;

/// <summary>
/// Calculates rental cost by splitting the booking into 30-minute slots
/// and applying the correct tariff multiplier to each slot.
/// </summary>
public class PricingService : IPricingService
{
    // Tariff time boundaries
    private static readonly TimeOnly MorningStart = new(6,  0);
    private static readonly TimeOnly MorningEnd   = new(9,  0);
    private static readonly TimeOnly PeakStart    = new(12, 0);
    private static readonly TimeOnly PeakEnd      = new(14, 0);
    private static readonly TimeOnly EveningStart = new(18, 0);
    private static readonly TimeOnly EveningEnd   = new(23, 0);

    // Tariff multipliers
    private const decimal MorningMultiplier = 0.90m; // -10%
    private const decimal StandardMultiplier = 1.00m; // base price
    private const decimal PeakMultiplier    = 1.15m; // +15%
    private const decimal EveningMultiplier = 0.80m; // -20%

    public decimal CalculateRoomCost(decimal baseHourlyRate, DateTime startTime, decimal durationHours)
    {
        const decimal slotSize = 0.5m; // 30-minute slots
        decimal totalCost = 0;
        var slotStart = startTime;
        var remaining = durationHours;

        while (remaining > 0)
        {
            var slotDuration   = Math.Min(slotSize, remaining);
            var multiplier     = GetRateMultiplier(TimeOnly.FromDateTime(slotStart));
            totalCost         += baseHourlyRate * multiplier * slotDuration;

            slotStart  = slotStart.AddMinutes(30);
            remaining -= slotDuration;
        }

        return Math.Round(totalCost, 2);
    }

    /// <summary>
    /// Returns the tariff multiplier for a given time.
    /// Priority (highest to lowest): Peak > Morning > Evening > Standard.
    /// </summary>
    private static decimal GetRateMultiplier(TimeOnly time)
    {
        if (time >= PeakStart    && time < PeakEnd)    return PeakMultiplier;
        if (time >= MorningStart && time < MorningEnd) return MorningMultiplier;
        if (time >= EveningStart && time < EveningEnd) return EveningMultiplier;
        return StandardMultiplier;
    }
}
