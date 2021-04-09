using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public int Number { get; set; }
        public string Addition { get; set; }

        // Lazy loading the city
        private ILazyLoader LazyLoader { get; set; }
        private City _city;
        [Required]
        public virtual City City
        {
            get => LazyLoader.Load(this, ref _city);
            set => _city = value;
        }
    }
}
