using System.ComponentModel.DataAnnotations;

namespace ABP_test.DTOs.Rooms;

/// <summary>
/// Data for updating an existing conference room.
/// Only provided fields will be updated (null = keep existing value).
/// </summary>
public class UpdateRoomDto
{
    /// <summary>New room name. Leave null to keep existing.</summary>
    [MaxLength(100, ErrorMessage = "Назва не може перевищувати 100 символів")]
    public string? Name { get; set; }

    /// <summary>New capacity. Leave null to keep existing.</summary>
    [Range(1, 10000, ErrorMessage = "Місткість повинна бути від 1 до 10000")]
    public int? Capacity { get; set; }

    /// <summary>New base hourly rate in UAH. Leave null to keep existing.</summary>
    [Range(0.01, 1_000_000, ErrorMessage = "Базова вартість повинна бути більше 0")]
    public decimal? BaseHourlyRate { get; set; }
}
