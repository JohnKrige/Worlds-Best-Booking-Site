using Winterflood.Server.Dtos.Bookings;
using Winterflood.Server.Entities;

namespace Winterflood.Server.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<GetBookingDto>> GetAllBookingsAsync(int UserId);
        void AddBooking(Booking booking);
        void RemoveBooking(Booking booking);
        Task<GetBookingDto?> GetBookingResponseIdAsync(int bookingId);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
    }
}
