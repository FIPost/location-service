using System;

namespace LocatieService.Database.Datamodels.Dtos
{
    public class RoomResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Building Building { get; set; }
    }
}
