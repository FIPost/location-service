using System;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels
{
    public class Locatie
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        public string Stad { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string Straat { get; set; }
        [Required]
        public int Nummer { get; set; }
        public string Toevoeging { get; set; }
    }
}
