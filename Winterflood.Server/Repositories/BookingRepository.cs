using Microsoft.EntityFrameworkCore;
using Winterflood.Server.Controllers;
using Winterflood.Server.Data;
using Winterflood.Server.Dtos.Bookings;
using Winterflood.Server.Entities;
using Winterflood.Server.Interfaces;

namespace Winterflood.Server.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetBookingDto>> GetAllBookingsAsync(int UserId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == UserId)
                .Select(b => new GetBookingDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    InventoryId = b.InventoryId,
                    Inventory = b.Inventory.Name,
                    TotalPrice = b.TotalPrice,
                    PricePerUnit = b.PricePerUnit,
                    NumberOfItems = b.NumberOfItems,
                    BookingEndDate = b.BookingEndDate,
                    BookingStartDate = b.BookingStartDate,
                    CreationDate = b.CreationDate,
                    EventDate = b.Inventory.EventDate,
                    Description = b.Inventory.Description
                })
                .ToListAsync();
        }

        public async Task<GetBookingDto?> GetBookingResponseIdAsync(int bookingId)
        {
            return _context.Bookings
                .Select(b => new GetBookingDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    InventoryId = b.InventoryId,
                    Inventory = b.Inventory.Name,
                    TotalPrice = b.TotalPrice,
                    PricePerUnit = b.PricePerUnit,
                    NumberOfItems = b.NumberOfItems,
                    BookingEndDate = b.BookingEndDate,
                    BookingStartDate = b.BookingStartDate,
                    CreationDate = b.CreationDate,
                    EventDate = b.Inventory.EventDate,
                    Description = b.Inventory.Description
                })
                .FirstOrDefault(b => b.Id == bookingId);
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return _context.Bookings
                .FirstOrDefault(b => b.Id == bookingId);
        }


        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
        }

        public void RemoveBooking(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }
    }
}
