using LocatieService.Database.Datamodels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocatieService.Database.Configuration
{
    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.ToTable("Buildings");
            builder.HasKey(x => x.Id);

            // Address object:
            builder.HasOne(x => x.Address);
        }
    }
}
