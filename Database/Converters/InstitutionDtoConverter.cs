using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System.Collections.Generic;

namespace LocatieService.Database.Converters
{
    public class InstitutionDtoConverter : IDtoConverter<Institution, InstitutionRequest, InstitutionResponse>
    {
        public Institution DtoToModel(InstitutionRequest request)
        {
            return new Institution
            {
                Name = request.Name,
                Addresses = request.Addresses
            };
        }

        public InstitutionResponse ModelToDto(Institution model)
        {
            return new InstitutionResponse
            {
                Id = model.Id,
                Name = model.Name,
                Addresses = model.Addresses
            };
        }

        public List<InstitutionResponse> ModelToDto(List<Institution> models)
        {
            List<InstitutionResponse> responseDtos = new();

            foreach (Institution institution in models)
            {
                responseDtos.Add(ModelToDto(institution));
            }

            return responseDtos;
        }
    }
}
