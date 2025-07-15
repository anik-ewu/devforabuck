using MediatR;
using DevForABuck.Application.Interfaces;
using DevForABuck.Domain.Entities;

namespace DevForABuck.Application.Queries.GetBookingsByEmail
{
    public class GetBookingsByEmailHandler : IRequestHandler<GetBookingsByEmailQuery, IEnumerable<Booking>>
    {
        private readonly IBookingService _bookingService;

        public GetBookingsByEmailHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IEnumerable<Booking>> Handle(GetBookingsByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _bookingService.GetBookingsByEmailAsync(request.Email);
        }
    }
}