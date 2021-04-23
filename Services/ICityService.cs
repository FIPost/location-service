using LocatieService.Database.Datamodels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public interface ICityService
    {
        Task<City> AddCityAsync(City city);
        Task<List<City>> GetAllCitiesAsync();
        Task<City> GetCityByIdAsync(Guid id);
        Task<City> GetCityByNameAsync(string name);
        Task<City> UpdateCityAsync(City city);
        Task<City> DeleteCityAsync(City city);
    }
}
