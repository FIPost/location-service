using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public class CityService : ICityService
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<City, CityRequest, CityResponse> _converter;

        public CityService(LocatieContext context,
            IDtoConverter<City, CityRequest, CityResponse> converter)
        {
            _context = context;
            _converter = converter;
        }

        public async Task<City> AddAsync(CityRequest request)
        {
            // Check if city is a duplicate:
            City city = _converter.DtoToModel(request);
            City duplicate = await _context.Cities.FirstOrDefaultAsync(e => e.Name == city.Name);

            if (duplicate != null)
            {
                throw new Exception($"City with name {city.Name} already exists.");
            }

            await _context.AddAsync(city);
            await _context.SaveChangesAsync();

            return city;
        }

        public async Task<List<City>> GetAllAsync()
        {
            return await _context.Cities.Where(e => e.IsActive).ToListAsync();
        }

        public async Task<City> GetByIdAsync(Guid id)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Id == id);

            if (city == null)
            {
                throw new Exception($"Could not find city with id {id}.");
            }

            return city;
        }

        public async Task<City> GetByNameAsync(string name)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Name == name);

            if (city == null)
            {
                throw new Exception($"Could not find city with name {name}.");
            }

            return city;
        }

        public async Task<City> UpdateAsync(Guid id, CityRequest request)
        {
            City city = _converter.DtoToModel(request);
            city.Id = id;

            _context.Update(city);
            await _context.SaveChangesAsync();

            return city;
        }

        public async Task DeleteAsync(Guid id)
        {
            // Set inactive:
            City city = await GetByIdAsync(id);
            city.IsActive = false;

            // Update record:
            _context.Update(city);
            await _context.SaveChangesAsync();
        }
    }
}
