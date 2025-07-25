using DevForABuck.Domain.Entities;
using MediatR;

namespace DevForABuck.Application.Queries;

public class GetAllSlots: IRequest<IEnumerable<AvailableSlot>>
{
    
}