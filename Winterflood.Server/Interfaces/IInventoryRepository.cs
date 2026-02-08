using Winterflood.Server.Dtos.Inventory;
using Winterflood.Server.Entities;

namespace Winterflood.Server.Interfaces
{
    public interface IInventoryRepository
    {
        Task<List<GetAllInventoryDto>> GetAllInventoryAsync();
        Task<Inventory?> GetInventoryByIdAsync(int id);
    }
}
