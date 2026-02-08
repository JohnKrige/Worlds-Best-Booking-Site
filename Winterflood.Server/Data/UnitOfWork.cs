using Winterflood.Server.Interfaces;
using Winterflood.Server.Repositories;

namespace Winterflood.Server.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IInventoryRepository Inventory { get; }
        public IBookingRepository Bookings { get; }

        public UnitOfWork(
            AppDbContext context, 
            IInventoryRepository inventoryRepository,
            IBookingRepository bookingRepository
        )
        {
            _context = context;
            Inventory = inventoryRepository;
            Bookings = bookingRepository;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}