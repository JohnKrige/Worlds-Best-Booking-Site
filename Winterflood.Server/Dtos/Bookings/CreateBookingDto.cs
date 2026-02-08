namespace Winterflood.Server.Dtos.Bookings
{
    public class CreateBookingDto
    {
        public int InventoryId { get; set; }
        public int NumberOfItems { get; set; } // Days rented out for
        public DateTime? BookingStartDate { get; set; }
        public DateTime? BookingEndDate { get; set; }
    }
}
