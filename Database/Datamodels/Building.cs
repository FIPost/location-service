using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels
{
    public class Building
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Lazy loading the address:
        private ILazyLoader LazyLoader { get; set; }
        private Address _address;
        public virtual Address Address
        {
            get => LazyLoader.Load(this, ref _address);
            set => _address = value;
        }
    }
}
