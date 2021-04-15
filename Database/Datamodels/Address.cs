using System;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid CityId { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public int Number { get; set; }
        public string Addition { get; set; }

        public Guid InstitutionId { get; set; }
        public Institution Institution { get; set; }
    }
}
