using MediatR;
using DevForABuck.Application.Interfaces;
using DevForABuck.Domain.Entities;

namespace DevForABuck.Application.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Booking>
    {
        private readonly IBookingService _bookingService;

        public CreateBookingCommandHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<Booking> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // Build Booking entity
            var booking = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                Stack = request.Stack,
                ExperienceYears = request.ExperienceYears,
                SlotTime = request.SlotTime
            };

            // Upload resume & save booking through your Infrastructure service
            using var stream = request.Resume.OpenReadStream();
            var result = await _bookingService.CreateBookingAsync(booking, stream, request.Resume.FileName);

            return result;
        }
    }
}