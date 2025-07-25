using DevForABuck.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevForABuck.API.Controllers;
[ApiController]
[Route("api/slots/commands")]
public class SlotsCommandController: ControllerBase
{
    private readonly IMediator _mediator;
    
    public SlotsCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSlot([FromBody] CreateAvailableSlotCommand command)
    {
        var slot = await _mediator.Send(command);
        return Ok(slot);
    }
}