using Newtonsoft.Json;

namespace DevForABuck.Domain.Entities;

public class AvailableSlot
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("startTime")]
    public DateTime StartTime { get; set; }

    [JsonProperty("endTime")]
    public DateTime EndTime { get; set; }

    [JsonProperty("isBooked")]
    public bool IsBooked { get; set; } = false;

    [JsonProperty("bookedByEmail")]
    public string? BookedByEmail { get; set; }    
    
    // Resume or Career council
    [JsonProperty("slotType")] 
    public string SlotType { get; set; } = "resume";
}