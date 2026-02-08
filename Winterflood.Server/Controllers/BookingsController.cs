using Microsoft.AspNetCore.Mvc;
using Winterflood.Server.Dtos.Bookings;
using Winterflood.Server.Entities;
using Winterflood.Server.Interfaces;

namespace Winterflood.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            int userId = 1; // Pretend this comes from the authenticated user context

            var bookings = await _unitOfWork.Bookings.GetAllBookingsAsync(userId);
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            int userId = 1; // Pretend this comes from the authenticated user context

            var booking = await _unitOfWork.Bookings.GetBookingResponseIdAsync(id);
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto createBookingDto)
        {
            int userId = 1; // Pretend this comes from the authenticated user context

            var inventoryItem = await _unitOfWork.Inventory.GetInventoryByIdAsync(createBookingDto.InventoryId);

            if (inventoryItem == null) return NotFound(new { message = "Inventory item not found." });

            if (createBookingDto.NumberOfItems > inventoryItem.TotalUnits) return BadRequest("Insufficient stock");

            if (
                inventoryItem.EventDate == null &&
                (createBookingDto.BookingStartDate == null || createBookingDto.BookingEndDate == null))
            {
                return BadRequest("Booking dates not valid");
            }

            var daysBooked = inventoryItem.EventDate != null ? 1 :
                GetDaysBooked(createBookingDto.BookingStartDate, createBookingDto.BookingEndDate);

            var newBooking = new Booking
            {
                UserId = userId,
                InventoryId = createBookingDto.InventoryId,
                NumberOfItems = createBookingDto.NumberOfItems,
                TotalPrice = createBookingDto.NumberOfItems * inventoryItem.PricePerDay * daysBooked,
                PricePerUnit = inventoryItem.PricePerDay,
                CreationDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                BookingStartDate = (DateTime)(inventoryItem.EventDate ?? createBookingDto.BookingStartDate!),
                BookingEndDate = (DateTime)(inventoryItem.EventDate ?? createBookingDto.BookingEndDate!)
            };

            _unitOfWork.Bookings.AddBooking(newBooking);

            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(
            nameof(GetBooking),
            new { id = newBooking.Id },
            GetBookingDto.MapFromBooking(newBooking)
        );
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateBookingStatus([FromBody] UpdateBookingDto updateBookingDto)
        {
            int userId = 1; // Pretend this comes from the authenticated user context

            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(updateBookingDto.BookingId);

            if (booking == null) return NotFound(new { message = "Booking not found." });

            if (booking.UserId != userId) return Forbid();

            var inventoryItem = await _unitOfWork.Inventory.GetInventoryByIdAsync(booking.InventoryId);

            if (inventoryItem == null) return NotFound(new { message = "Inventory item not found." });

            var daysBooked = inventoryItem.EventDate != null ? 1 :
                GetDaysBooked(updateBookingDto.BookingStartDate, updateBookingDto.BookingEndDate);

            booking.LastModified = DateTime.UtcNow;
            booking.BookingStartDate = updateBookingDto.BookingStartDate;
            booking.BookingEndDate = updateBookingDto.BookingEndDate;
            booking.NumberOfItems = updateBookingDto.NumberOfItems;
            booking.TotalPrice = updateBookingDto.NumberOfItems * booking.PricePerUnit * daysBooked;

            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            int userId = 1; // Pretend this comes from the authenticated user context;

            var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);

            if (booking == null) return NotFound(new { message = "Booking not found." });

            if (booking.UserId != userId) return Forbid();

            _unitOfWork.Bookings.RemoveBooking(booking);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private int GetDaysBooked(DateTime? startDate, DateTime? endDate)
        {
            int daysBooked = 1;

            if (startDate != null && endDate != null)
            {
                daysBooked = DateOnly.FromDateTime((DateTime)endDate).DayNumber -
                    DateOnly.FromDateTime((DateTime)startDate).DayNumber;
            }

            if (daysBooked >= 1) return daysBooked;

            throw new Exception("Invalid dates provided");
        }
    }
}
