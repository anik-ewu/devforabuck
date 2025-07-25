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
    
    public Task<IEnumerable<AvailableSlot>> Handle(GetAllSlots request, CancellationToken cancellationToken)
    {
        try
        {
            var allSlots = _slotService.GetAllSlotsAsync();
            return allSlots;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}