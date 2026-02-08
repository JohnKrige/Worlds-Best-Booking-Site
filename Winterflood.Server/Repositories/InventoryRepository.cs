using Microsoft.EntityFrameworkCore;
using Winterflood.Server.Data;
using Winterflood.Server.Dtos.Inventory;
using Winterflood.Server.Entities;
using Winterflood.Server.Interfaces;

namespace Winterflood.Server.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllInventoryDto>> GetAllInventoryAsync()
        {
            return await _context.Inventory
                .Select(i => new GetAllInventoryDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    PricePerDay = i.PricePerDay,
                    InventoryTypeId = i.InventoryTypeId,
                    InventoryType = i.InventoryType.Name,
                    Description = i.Description,
                    EventDate = i.EventDate,
                    AvailableItems = i.TotalUnits
                })
                .ToListAsync();

        }

        public async Task<Inventory?> GetInventoryByIdAsync(int id)
        {
            return await _context.Inventory
                .Include(i => i.InventoryType)
                //.Include(i => i.Bookings)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
