﻿using System;

namespace LocatieService.Database.Datamodels.Dtos
{
    public class BuildingResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }
}