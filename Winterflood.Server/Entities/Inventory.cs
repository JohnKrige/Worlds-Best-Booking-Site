namespace Winterflood.Server.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerDay { get; set; }
        public int InventoryTypeId { get; set; }
        public InventoryType InventoryType { get; set; }
        public string Description { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public int TotalUnits { get; set; }
        public DateTime? EventDate { get; set; }
    }
}
