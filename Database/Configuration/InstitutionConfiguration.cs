using LocatieService.Database.Datamodels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocatieService.Database.Configuration
{
    public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
    {
        public void Configure(EntityTypeBuilder<Institution> builder)
        {
            builder.ToTable("Institutions");
            builder.HasKey(x => x.Id);

            // Address objects:
            builder.HasMany(x => x.Addresses);
        }
    }
}
