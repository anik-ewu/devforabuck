using DevForABuck.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevForABuck.API.Controllers;
[ApiController]
[Route("api/slots/queries")]
public class SlotsQueryController: ControllerBase
{
    private readonly IMediator _mediator;

    public SlotsQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("admin/allSlots")]
    public async Task<IActionResult> GetAllSlotsAsync()
    {
        var allSlots = await _mediator.Send(new GetAllSlots());
        return Ok(allSlots);
    }
}