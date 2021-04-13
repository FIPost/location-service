using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocatieService.Database.Datamodels
{
    public class Institution
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Lazy loading the addresses:
        private ILazyLoader LazyLoader { get; set; }
        private ICollection<Address> _addresses;
        public virtual ICollection<Address> Addresses
        {
            get => LazyLoader.Load(this, ref _addresses);
            set => _addresses = value;
        }
    }
}
