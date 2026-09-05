using System.ComponentModel.DataAnnotations;

namespace ABP_test.DTOs.Rooms;

/// <summary>
/// Data required to create a new conference room.
/// </summary>
public class CreateRoomDto
{
    /// <summary>Room display name (e.g. "Зал A").</summary>
    [Required(ErrorMessage = "Назва залу є обов'язковою")]
    [MaxLength(100, ErrorMessage = "Назва не може перевищувати 100 символів")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Maximum number of people the room can accommodate.</summary>
    [Range(1, 10000, ErrorMessage = "Місткість повинна бути від 1 до 10000")]
    public int Capacity { get; set; }

    /// <summary>Base rental price per hour in UAH.</summary>
    [Range(0.01, 1_000_000, ErrorMessage = "Базова вартість повинна бути більше 0")]
    public decimal BaseHourlyRate { get; set; }
}
