using DevForABuck.Application.Interfaces;
using DevForABuck.Domain.Entities;
using MediatR;

namespace DevForABuck.Application.Queries;

public class GetAllSlotsHandler: IRequestHandler<GetAllSlots, IEnumerable<AvailableSlot>>
{
    private readonly ISlotService _slotService;

    public GetAllSlotsHandler(ISlotService slotService)
    {
        _slotService = slotService;
    }
    
    public async Task<IEnumerable<AvailableSlot>> Handle(GetAllSlots request, CancellationToken cancellationToken)
    {
        var allSlots = await _slotService.GetAllSlotsAsync();
        return allSlots;
    }
}