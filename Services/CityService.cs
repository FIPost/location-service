using LocatieService.Database.Datamodels;
using LocatieService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _repository;

        public CityService(ICityRepository repository)
        {
            _repository = repository;
        }

        public async Task<City> AddCityAsync(City city)
        {
            return await _repository.AddAsync(city);
        }

        public async Task<City> DeleteCityAsync(City city)
        {
            return await _repository.DeleteAsync(city);
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<City> GetCityByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<City> GetCityByNameAsync(string name)
        {
            return await _repository.GetByNameAsync(name);
        }

        public async Task<City> UpdateCityAsync(City city)
        {
            return await _repository.UpdateAsync(city);
        }
    }
}
