using System;

namespace LocatieService.Database.Datamodels.Dtos
{
    public class AddressResponse
    {
        public Guid Id { get; set; }
        public City City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Addition { get; set; }
    }
}
