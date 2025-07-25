using DevForABuck.Domain.Entities;
using MediatR;

namespace DevForABuck.Application.Commands;

public class CreateAvailableSlotCommand(DateTime startTime, DateTime endTime, string slotType) : IRequest<AvailableSlot>
{
    public DateTime StartTime { get; set; } = startTime;
    public DateTime EndTime { get; set; } = endTime;
    public string SlotType { get; set; } = slotType;
}