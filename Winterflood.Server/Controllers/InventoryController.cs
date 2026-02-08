using Microsoft.AspNetCore.Mvc;
using Winterflood.Server.Interfaces;

namespace Winterflood.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventory()
        {
            var inventoryItems = await _unitOfWork.Inventory.GetAllInventoryAsync();
            return Ok(inventoryItems);
        }
    }
}
