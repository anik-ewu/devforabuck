using MediatR;
using DevForABuck.Domain.Entities;
using System.Collections.Generic;

namespace DevForABuck.Application.Queries.GetAllBookings
{
    public class GetAllBookingsQuery : IRequest<IEnumerable<Booking>>
    {
        // No params needed for all bookings
    }
}