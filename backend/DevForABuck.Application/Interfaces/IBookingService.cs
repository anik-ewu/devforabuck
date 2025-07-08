// using DevForABuck.Domain.Entities;

namespace DevForABuck.Application.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(Booking booking, Stream resumeStream, string fileName);
        Task<IEnumerable<Booking>> GetBookingsByEmailAsync(string email);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
    }
}