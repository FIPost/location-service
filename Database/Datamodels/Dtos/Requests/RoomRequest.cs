using System;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels.Dtos
{
    public class RoomRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Building Building { get; set; }
    }
}
