using Winterflood.Server.Entities;

namespace Winterflood.Server.Dtos.Bookings
{
    public class GetBookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int InventoryId { get; set; }
        public string Inventory { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public decimal PricePerUnit { get; set; }
        public int NumberOfItems { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EventDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int AvailableItems { get; set; }


        public static GetBookingDto MapFromBooking(Booking b)
        {
            return new GetBookingDto
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
                Description = b.Inventory.Description,
                AvailableItems = b.Inventory.TotalUnits
            };
        }
    }
}
