using Newtonsoft.Json;
namespace DevForABuck.Domain.Entities;

public class Booking
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("stack")]
    public string Stack { get; set; }

    [JsonProperty("experienceYears")]
    public int ExperienceYears { get; set; }

    [JsonProperty("slotTime")]
    public DateTime SlotTime { get; set; }
    
    [JsonProperty("sessionType")]
    public string SessionType { get; set; }
    
    [JsonProperty("resumeUrl")]
    public string ResumeUrl { get; set; }
}