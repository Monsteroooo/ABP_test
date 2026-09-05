namespace ABP_test.Services.Interfaces;

public interface IPricingService
{
    /// <summary>
    /// Calculates the total room rental cost by splitting the booking into
    /// 30-minute slots and applying the appropriate tariff to each slot.
    /// </summary>
    /// <param name="baseHourlyRate">Room base price per hour in UAH.</param>
    /// <param name="startTime">Booking start date and time.</param>
    /// <param name="durationHours">Duration in hours (multiples of 0.5).</param>
    /// <returns>Total room rental cost in UAH.</returns>
    decimal CalculateRoomCost(decimal baseHourlyRate, DateTime startTime, decimal durationHours);
}
