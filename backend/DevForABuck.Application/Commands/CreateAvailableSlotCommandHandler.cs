using DevForABuck.Application.Interfaces;
using DevForABuck.Domain.Entities;
using MediatR;

namespace DevForABuck.Application.Commands;

public class CreateAvailableSlotCommandHandler: IRequestHandler<CreateAvailableSlotCommand, AvailableSlot>
{
    private readonly ISlotService _slotService;

    public CreateAvailableSlotCommandHandler(ISlotService slotService)
    {
        _slotService = slotService;
    }
    
    
    public async Task<AvailableSlot> Handle(CreateAvailableSlotCommand request, CancellationToken cancellationToken)
    {
        var payload = new AvailableSlot
        {
            Id = Guid.NewGuid().ToString(),
            SlotType = request.SlotType,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            BookedByEmail = "sabbirhasananik@gmail.com",
            IsBooked = false
        };

        var result = await _slotService.CreateSlotAsync(payload);
        return result;
    }
}