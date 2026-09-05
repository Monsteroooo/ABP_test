using System.ComponentModel.DataAnnotations;

namespace ABP_test.DTOs.Rooms;

/// <summary>
/// Query parameters for searching available conference rooms.
/// </summary>
public class SearchRoomsDto
{
    /// <summary>Date of the booking (e.g. 2024-09-01).</summary>
    [Required(ErrorMessage = "Дата є обов'язковою")]
    public DateOnly Date { get; set; }

    /// <summary>Desired start time (e.g. 10:00).</summary>
    [Required(ErrorMessage = "Час початку є обов'язковим")]
    public TimeOnly StartTime { get; set; }

    /// <summary>Desired end time (e.g. 14:00).</summary>
    [Required(ErrorMessage = "Час закінчення є обов'язковим")]
    public TimeOnly EndTime { get; set; }

    /// <summary>Minimum required capacity.</summary>
    [Range(1, 10000, ErrorMessage = "Місткість повинна бути від 1 до 10000")]
    public int Capacity { get; set; }
}
