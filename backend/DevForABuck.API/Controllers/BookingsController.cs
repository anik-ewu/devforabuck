using DevForABuck.Application.Interfaces;
using DevForABuck.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DevForABuck.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            [FromForm] string name,
            [FromForm] string email,
            [FromForm] string stack,
            [FromForm] int experienceYears,
            [FromForm] DateTime slotTime,
            [FromForm] IFormFile resume)
        {
            if (resume == null || resume.Length == 0)
                return BadRequest("Resume file is required.");

            var booking = new Booking
            {
                Name = name,
                Email = email,
                Stack = stack,
                ExperienceYears = experienceYears,
                SlotTime = slotTime
            };

            // ✅ Open stream from IFormFile
            using var stream = resume.OpenReadStream();

            // ✅ Pass Stream + FileName to Service
            var createdBooking = await _bookingService.CreateBookingAsync(booking, stream, resume.FileName);

            return CreatedAtAction(nameof(CreateBooking), new { id = createdBooking.Id }, createdBooking);
        }



        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var bookings = await _bookingService.GetBookingsByEmailAsync(email);
            return Ok(bookings);
        }

        [HttpGet("admin/all")]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }
    }
}