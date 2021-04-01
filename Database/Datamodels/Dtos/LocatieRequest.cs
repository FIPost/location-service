using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels.Dtos
{
    public class LocatieRequest
    {
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
