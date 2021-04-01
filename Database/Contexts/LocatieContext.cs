using LocatieService.Database.Datamodels;
using Microsoft.EntityFrameworkCore;

namespace LocatieService.Database.Contexts
{
    public class LocatieContext : DbContext
    {
        public LocatieContext(DbContextOptions<LocatieContext> options) : base(options) { }

        public DbSet<Locatie> Locaties { get; set; }
    }
}
