using DevForABuck.API.Models;
using DevForABuck.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevForABuck.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBooking([FromForm] BookingRequest request)
        {
            _logger.LogInformation("📌 [CreateBooking] Name: {Name}", request.Name);
            _logger.LogInformation("📌 [CreateBooking] Resume is null? {IsNull}", request.Resume == null);

            if (request.Resume == null || request.Resume.Length == 0)
            {
                _logger.LogWarning("❌ [CreateBooking] Resume file is missing.");
                return BadRequest("Resume file is required.");
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                Stack = request.Stack,
                ExperienceYears = request.ExperienceYears,
                SlotTime = request.SlotTime
            };

            try
            {
                using var stream = request.Resume.OpenReadStream();
                var createdBooking = await _bookingService.CreateBookingAsync(booking, stream, request.Resume.FileName);

                if (createdBooking == null)
                {
                    _logger.LogError("❌ [CreateBooking] Failed to save booking to database.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to save booking to database.");
                }

                _logger.LogInformation("✅ [CreateBooking] Booking created with ID: {Id}", createdBooking.Id);
                return CreatedAtAction(nameof(CreateBooking), createdBooking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ [CreateBooking] Exception occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating booking.");
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            _logger.LogInformation("📌 [GetByEmail] Fetching bookings for email: {Email}", email);

            try
            {
                var bookings = await _bookingService.GetBookingsByEmailAsync(email);
                _logger.LogInformation("✅ [GetByEmail] Retrieved {Count} bookings.", bookings?.Count() ?? 0);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ [GetByEmail] Exception occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching bookings.");
            }
        }

        [HttpGet("admin/all")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("📌 [GetAll] Called");

            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                _logger.LogInformation("✅ [GetAll] Retrieved {Count} bookings.", bookings?.Count() ?? 0);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ [GetAll] Exception occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching bookings.");
            }
        }
    }
}
