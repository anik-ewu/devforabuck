using DevForABuck.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/bookings/commands")]
public class BookingsCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateBookingCommand command)
    {
        var booking = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), booking);
    }
}