using Microsoft.EntityFrameworkCore;
using Winterflood.Server.Entities;

namespace Winterflood.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Inventory> Inventory => Set<Inventory>();
        public DbSet<InventoryType> InventoryTypes => Set<InventoryType>();
    }
}
