using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatieService.Database.Datamodels
{
    public class Building
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }
}
