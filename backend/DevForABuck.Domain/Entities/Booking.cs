namespace DevForABuck.Domain.Entities
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Stack { get; set; }
        public int ExperienceYears { get; set; }
        public DateTime SlotTime { get; set; }
        public string ResumeUrl { get; set; }
        public string Status { get; set; } = "Pending";
    }
}