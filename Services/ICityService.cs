using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public interface ICityService
    {
        Task<ActionResult<City>> AddAsync(CityRequest request);
        Task<ActionResult<List<City>>> GetAllAsync();
        Task<ActionResult<City>> GetByIdAsync(Guid id);
        Task<ActionResult<City>> GetByNameAsync(string name);
        Task<ActionResult<City>> UpdateAsync(Guid id, CityRequest request);
        Task<ActionResult> DeleteAsync(Guid id);
    }
}
