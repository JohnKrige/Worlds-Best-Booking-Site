namespace Winterflood.Server.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PricePerUnit { get; set; }
        public int NumberOfItems { get; set; }
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
