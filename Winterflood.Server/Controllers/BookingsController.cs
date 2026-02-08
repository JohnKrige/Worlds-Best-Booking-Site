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
            if (booking == null) return NotFound();

            if (booking.UserId != userId) return Forbid();

            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto createBookingDto)
        {
            int userId = 1; // Pretend this comes from the authenticated user context

            var inventoryItem = await _unitOfWork.Inventory.GetInventoryByIdAsync(createBookingDto.InventoryId);

            if (inventoryItem == null) return NotFound();

            if (createBookingDto.NumberOfItems > inventoryItem.TotalUnits || createBookingDto.NumberOfItems <= 0 ) return BadRequest();

            if (
                inventoryItem.EventDate == null &&
                (createBookingDto.BookingStartDate == null || createBookingDto.BookingEndDate == null))
            {
                return BadRequest("Booking dates not valid");
            }

            if (inventoryItem.EventDate == null && createBookingDto.BookingEndDate <= createBookingDto.BookingStartDate)
                return BadRequest("End date must be after start date");

            var daysBooked = inventoryItem.EventDate != null ? 1 :
                GetDaysBooked((DateTime)createBookingDto.BookingStartDate!, (DateTime)createBookingDto.BookingEndDate!);

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

            if (booking == null) return NotFound();

            if (booking.UserId != userId) return Forbid();

            var inventoryItem = await _unitOfWork.Inventory.GetInventoryByIdAsync(booking.InventoryId);

            if (inventoryItem == null) return NotFound();

            if (updateBookingDto.NumberOfItems > inventoryItem.TotalUnits ||
                updateBookingDto.NumberOfItems <= 0
            ) return BadRequest();

            if (
                inventoryItem.EventDate == null &&
                (updateBookingDto.BookingStartDate == null || updateBookingDto.BookingEndDate == null)
            )
            {
                return BadRequest("Booking dates not valid");
            }

            if (inventoryItem.EventDate == null && updateBookingDto.BookingEndDate <= updateBookingDto.BookingStartDate)
                return BadRequest("End date must be after start date");


            var daysBooked = inventoryItem.EventDate != null ? 1 :
                GetDaysBooked((DateTime)updateBookingDto.BookingStartDate!, (DateTime)updateBookingDto.BookingEndDate!);

            booking.LastModified = DateTime.UtcNow;
            booking.BookingStartDate = (DateTime)(inventoryItem.EventDate ?? updateBookingDto.BookingStartDate!);
            booking.BookingEndDate = (DateTime)(inventoryItem.EventDate ?? updateBookingDto.BookingEndDate!);
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

            if (booking == null) return NotFound();

            if (booking.UserId != userId) return Forbid();

            _unitOfWork.Bookings.RemoveBooking(booking);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private int GetDaysBooked(DateTime startDate, DateTime endDate)
        {
            return DateOnly.FromDateTime((DateTime)endDate).DayNumber -
                DateOnly.FromDateTime((DateTime)startDate).DayNumber;
        }
    }
}
