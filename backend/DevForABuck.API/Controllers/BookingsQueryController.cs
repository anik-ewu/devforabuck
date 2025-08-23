using DevForABuck.Application.Queries.GetAllBookings;
using DevForABuck.Application.Queries.GetBookingsByEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevForABuck.API.Controllers
{
    [ApiController]
    [Route("api/bookings/queries")]
    public class BookingsQueryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _mediator.Send(new GetAllBookingsQuery());
            return Ok(bookings);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetBookingsByEmail(string email)
        {
            var result = await _mediator.Send(new GetBookingsByEmailQuery { Email = email });
            return Ok(result);
        }
}
}