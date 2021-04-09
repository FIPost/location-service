using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels
{
    public class Building
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        // Lazy loading the city
        private ILazyLoader LazyLoader { get; set; }
        private Address _address;
        [Required]
        public virtual Address Address
        {
            get => LazyLoader.Load(this, ref _address);
            set => _address = value;
        }
    }
}
