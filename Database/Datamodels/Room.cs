using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Lazy loading the building:
        private ILazyLoader LazyLoader { get; set; }
        private Building _building;
        public Building Building
        {
            get => LazyLoader.Load(this, ref _building);
            set => _building = value;
        }
    }
}
