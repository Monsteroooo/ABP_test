using System.ComponentModel.DataAnnotations;

namespace ABP_test.DTOs.Bookings;

/// <summary>
/// Data required to create a new booking.
/// </summary>
public class CreateBookingDto
{
    /// <summary>ID of the room to book.</summary>
    [Required(ErrorMessage = "ID залу є обов'язковим")]
    public int RoomId { get; set; }

    /// <summary>Date of the booking.</summary>
    [Required(ErrorMessage = "Дата є обов'язковою")]
    public DateOnly Date { get; set; }

    /// <summary>Start time of the booking.</summary>
    [Required(ErrorMessage = "Час початку є обов'язковим")]
    public TimeOnly StartTime { get; set; }

    /// <summary>Duration in hours. Must be a multiple of 0.5 (e.g. 1, 1.5, 2).</summary>
    [Range(0.5, 17.0, ErrorMessage = "Тривалість повинна бути від 0.5 до 17 годин")]
    public decimal DurationHours { get; set; }

    /// <summary>List of service IDs to include in the booking (optional).</summary>
    public List<int> ServiceIds { get; set; } = [];
}
