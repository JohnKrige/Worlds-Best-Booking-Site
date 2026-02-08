namespace Winterflood.Server.Entities
{
    public class InventoryType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Inventory> InventoryItems { get; set; } = [];
    }
}
