﻿using System;
using System.Collections.Generic;

namespace LocatieService.Database.Datamodels.Dtos
{
    public class InstitutionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}