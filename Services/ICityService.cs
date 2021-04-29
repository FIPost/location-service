using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public interface ICityService
    {
        Task<City> AddAsync(CityRequest request);
        Task<List<City>> GetAllAsync();
        Task<City> GetByIdAsync(Guid id);
        Task<City> GetByNameAsync(string name);
        Task<City> UpdateAsync(Guid id, CityRequest request);
        Task<City> DeleteAsync(Guid id);
    }
}
