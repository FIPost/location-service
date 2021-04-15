using System;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels
{
    public class City
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
