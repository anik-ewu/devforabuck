using MediatR;
using DevForABuck.Domain.Entities;
using System.Collections.Generic;

namespace DevForABuck.Application.Queries.GetBookingsByEmail
{
    public class GetBookingsByEmailQuery : IRequest<IEnumerable<Booking>>
    {
        public string Email { get; set; } = string.Empty;
    }
}