using DevForABuck.Domain.Entities;

namespace DevForABuck.Application.Interfaces;

public interface ISlotService
{
    Task<AvailableSlot> CreateSlotAsync(AvailableSlot slot);
    Task<IEnumerable<AvailableSlot>> GetAllSlotsAsync();
}