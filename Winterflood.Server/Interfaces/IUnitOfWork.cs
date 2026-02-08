namespace Winterflood.Server.Interfaces
{
    public interface IUnitOfWork
    {
        public IInventoryRepository Inventory { get; }
        public IBookingRepository Bookings { get; }
        Task<int> SaveChangesAsync();
    }
}
