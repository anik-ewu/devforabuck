using DevForABuck.API.Models;
using DevForABuck.Application.Interfaces;
// using DevForABuck.Domain.Entities;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBooking([FromForm] BookingRequest request)
        {
            Console.WriteLine($"Name: {request.Name}");
            Console.WriteLine($"Resume is null? {request.Resume == null}");

            if (request.Resume == null || request.Resume.Length == 0)
                return BadRequest("Resume file is required.");

            var booking = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                Stack = request.Stack,
                ExperienceYears = request.ExperienceYears,
                SlotTime = request.SlotTime
            };

            using var stream = request.Resume.OpenReadStream();
            var createdBooking = await _bookingService.CreateBookingAsync(booking, stream, request.Resume.FileName);

            if (createdBooking == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to save booking to database.");
            }

            return CreatedAtAction(nameof(CreateBooking), createdBooking);
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
            Console.WriteLine("📌 [BookingsController] GetAll called");

            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                Console.WriteLine($"✅ [BookingsController] Retrieved {bookings?.Count() ?? 0} bookings.");

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [BookingsController] Exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching bookings.");
            }
        }

    }
}