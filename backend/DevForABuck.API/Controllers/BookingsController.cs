using DevForABuck.API.Models;
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
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBooking([FromForm] BookingRequest request)
        {
            if (request.Resume == null || request.Resume.Length == 0)
                return BadRequest("Resume file is required.");

            var booking = new Booking
            {
                Name = request.Name,
                Email = request.Email,
                Stack = request.Stack,
                ExperienceYears = request.ExperienceYears,
                SlotTime = request.SlotTime
            };

            using var stream = request.Resume.OpenReadStream();
            var createdBooking = await _bookingService.CreateBookingAsync(booking, stream, request.Resume.FileName);

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