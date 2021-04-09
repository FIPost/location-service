using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        // Lazy loading the city
        private ILazyLoader LazyLoader { get; set; }
        private Building _building;
        [Required]
        public virtual Building Building
        {
            get => LazyLoader.Load(this, ref _building);
            set => _building = value;
        }
    }
}
