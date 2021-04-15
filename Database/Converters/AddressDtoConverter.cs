using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System.Collections.Generic;

namespace LocatieService.Database.Converters
{
    public class AddressDtoConverter : IDtoConverter<Address, AddressRequest, AddressResponse>
    {
        public Address DtoToModel(AddressRequest request)
        {
            return new Address
            {
                CityId = request.CityId,
                PostalCode = request.PostalCode,
                Street = request.Street,
                Number = request.Number,
                Addition = request.Addition
            };
        }

        public AddressResponse ModelToDto(Address model)
        {
            return new AddressResponse
            {
                Id = model.Id,
                // City wordt in controller opgehaald.
                PostalCode = model.PostalCode,
                Street = model.Street,
                Number = model.Number,
                Addition = model.Addition
            };
        }

        public List<AddressResponse> ModelToDto(List<Address> models)
        {
            List<AddressResponse> responseDtos = new();

            foreach (Address address in models)
            {
                responseDtos.Add(ModelToDto(address));
            }

            return responseDtos;
        }
    }
}
