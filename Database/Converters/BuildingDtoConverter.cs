using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System.Collections.Generic;

namespace LocatieService.Database.Converters
{
    public class BuildingDtoConverter : IDtoConverter<Building, BuildingRequest, BuildingResponse>
    {
        public Building DtoToModel(BuildingRequest request)
        {
            return new Building
            {
                Name = request.Name,
                AddressId = request.AddressId
            };
        }

        public BuildingResponse ModelToDto(Building model)
        {
            return new BuildingResponse
            {
                Id = model.Id,
                Name = model.Name,
                // Address wordt in controller opgehaald.
            };
        }

        public List<BuildingResponse> ModelToDto(List<Building> models)
        {
            List<BuildingResponse> responseDtos = new();

            foreach (Building building in models)
            {
                responseDtos.Add(ModelToDto(building));
            }

            return responseDtos;
        }
    }
}
