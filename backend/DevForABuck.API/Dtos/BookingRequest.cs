using Microsoft.AspNetCore.Http;
using System;

namespace DevForABuck.API.Models
{
    public class BookingRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Stack { get; set; }
        public int ExperienceYears { get; set; }
        public DateTime SlotTime { get; set; }
        public IFormFile Resume { get; set; }
    }
}