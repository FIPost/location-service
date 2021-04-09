using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System;
using System.Collections.Generic;

namespace LocatieService.Database.Converters
{
    public class RoomDtoConverter : IDtoConverter<Room, RoomRequest, RoomResponse>
    {
        public Room DtoToModel(RoomRequest request)
        {
            return new Room
            {
                Name = request.Name,
                // Building will be added in controller after fetching from db.
            };
        }

        public RoomResponse ModelToDto(Room model)
        {
            return new RoomResponse
            {
                Id = model.Id,
                Name = model.Name,
                Building = model.Building
            };
        }

        public List<RoomResponse> ModelToDto(List<Room> models)
        {
            List<RoomResponse> responseDtos = new();

            foreach (Room room in models)
            {
                responseDtos.Add(ModelToDto(room));
            }

            return responseDtos;
        }
    }
}
