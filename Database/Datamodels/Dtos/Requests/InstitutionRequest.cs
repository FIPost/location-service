using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels.Dtos
{
    public class InstitutionRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Address> Addresses { get; set; }
    }
}
