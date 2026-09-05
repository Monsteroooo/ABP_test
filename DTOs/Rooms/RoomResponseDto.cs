namespace ABP_test.DTOs.Rooms;

/// <summary>
/// Room data returned to the client after creation or retrieval.
/// </summary>
public class RoomResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BaseHourlyRate { get; set; }
}
