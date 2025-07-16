using MediatR;
using DevForABuck.Application.Interfaces;
using DevForABuck.Domain.Entities;

namespace DevForABuck.Application.Queries.GetAllBookings
{
    public class GetAllBookingsHandler : IRequestHandler<GetAllBookingsQuery, IEnumerable<Booking>>
    {
        private readonly IBookingService _bookingService;

        public GetAllBookingsHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IEnumerable<Booking>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            return await _bookingService.GetAllBookingsAsync();
        }
    }
}