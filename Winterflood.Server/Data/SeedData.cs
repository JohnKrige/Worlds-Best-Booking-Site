using Microsoft.EntityFrameworkCore;
using Winterflood.Server.Entities;

namespace Winterflood.Server.Data
{
    public class SeedData
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            await db.Database.MigrateAsync();

            if (!await db.Users.AnyAsync())
            {
                db.Users.AddRange(
                    new User { Username = "John Doe" }
                );

                await db.SaveChangesAsync();
            }

            if (!await db.Inventory.AnyAsync())
            {

                var accommodation = new InventoryType { Name = "Accommodation" };
                var vehicles = new InventoryType { Name = "Vehicles" };
                var shows = new InventoryType { Name = "Shows" };


                db.InventoryTypes.AddRange(accommodation, vehicles, shows);
                await db.SaveChangesAsync();

                var inventoryItems = new List<Inventory>
                {
                    new Inventory { 
                        Name = "Cabin A",
                        InventoryType = accommodation,
                        PricePerDay = 3200,
                        Description = "A tailored and highly persuasive descriptiopn of the item designed to move consumers through a sales funnel by highlighting the unique value of the product",
                        TotalUnits = 1
                    },
                    new Inventory { 
                        Name = "Cabin B", 
                        InventoryType = accommodation, 
                        PricePerDay = 2500,
                       Description = "A tailored and highly persuasive descriptiopn of the item designed to move consumers through a sales funnel by highlighting the unique value of the product",
                       TotalUnits = 1

                    },
                    new Inventory { 
                        Name = "Beach Bike", 
                        InventoryType = vehicles,
                        PricePerDay = 1000,
                        Description = "A tailored and highly persuasive descriptiopn of the item designed to move consumers through a sales funnel by highlighting the unique value of the product",
                        TotalUnits = 4,
                    },
                    new Inventory { 
                        Name = "Kayak", 
                        InventoryType = vehicles,
                        PricePerDay = 1000,
                        Description = "A tailored and highly persuasive descriptiopn of the item designed to move consumers through a sales funnel by highlighting the unique value of the product",
                        TotalUnits = 12
                    },
                    new Inventory { 
                        Name = "Winter Festival Ticket",
                        InventoryType = shows,
                        PricePerDay = 200,
                        Description = "A tailored and highly persuasive descriptiopn of the item designed to move consumers through a sales funnel by highlighting the unique value of the product",
                        TotalUnits = 500,
                        EventDate = DateTime.Now.AddDays(120)
                    },
                    new Inventory { 
                        Name = "The Big Bokjol Show",
                        InventoryType = shows,
                        PricePerDay = 500,
                        Description = "A tailored and highly persuasive descriptiopn of the item designed to move consumers through a sales funnel by highlighting the unique value of the product",
                        TotalUnits = 1000,
                        EventDate = DateTime.Now.AddDays(30)
                    }
                };

                db.Inventory.AddRange(inventoryItems);
                await db.SaveChangesAsync();
            }
        }
    }
}
