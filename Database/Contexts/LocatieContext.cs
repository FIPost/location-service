using LocatieService.Database.Datamodels;
using Microsoft.EntityFrameworkCore;

namespace LocatieService.Database.Contexts
{
    public class LocatieContext : DbContext
    {
        public LocatieContext(DbContextOptions<LocatieContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }
}
