using Winterflood.Server.Entities;

namespace Winterflood.Server.Dtos.Inventory
{
    public class GetAllInventoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerDay { get; set; }
        public int InventoryTypeId { get; set; }
        public string InventoryType { get; set; }
        public string Description { get; set; }
        public DateTime? EventDate { get; set; }
        public int AvailableItems { get; set; }
    }
}
