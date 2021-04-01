using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System.Collections.Generic;

namespace LocatieService.Database.Converters
{
    public class DtoConverter : IDtoConverter<Locatie, LocatieRequest, LocatieResponse>
    {
        public Locatie DtoToModel(LocatieRequest request)
        {
            return new Locatie
            {
                Naam = request.Naam,
                Stad = request.Stad,
                Postcode = request.Postcode,
                Straat = request.Straat,
                Nummer = request.Nummer,
                Toevoeging = request.Toevoeging
            };
        }

        public LocatieResponse ModelToDto(Locatie model)
        {
            return new LocatieResponse
            {
                Id = model.Id,
                Naam = model.Naam,
                Stad = model.Stad,
                Postcode = model.Postcode,
                Straat = model.Straat,
                Nummer = model.Nummer,
                Toevoeging = model.Toevoeging
            };
        }

        public List<LocatieResponse> ModelToDto(List<Locatie> locaties)
        {
            List<LocatieResponse> responseDtos = new();

            foreach (Locatie locatie in locaties)
            {
                responseDtos.Add(ModelToDto(locatie));
            }

            return responseDtos;
        }
    }
}
