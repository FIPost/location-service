using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace LocatieService.Database.Datamodels
{
    public class Address
    {
        public Guid Id { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Addition { get; set; }

        // Lazy loading the city:
        private ILazyLoader LazyLoader { get; set; }
        private City _city;
        public virtual City City
        {
            get => LazyLoader.Load(this, ref _city);
            set => _city = value;
        }
    }
}
