using DevForABuck.Application.Queries.GetAllBookings;
using MediatR;
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
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _mediator.Send(new GetAllBookingsQuery());
            return Ok(bookings);
        }
    }
}