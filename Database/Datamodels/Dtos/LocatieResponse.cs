using System;

namespace LocatieService.Database.Datamodels.Dtos
{
    public class LocatieResponse
    {
        public Guid Id { get; set; }
        public string Naam { get; set; }
        public string Stad { get; set; }
        public string Postcode { get; set; }
        public string Straat { get; set; }
        public int Nummer { get; set; }
        public string Toevoeging { get; set; }
    }
}
