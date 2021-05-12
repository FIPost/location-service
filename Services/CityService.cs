using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using LocatieService.helpers;
using Microsoft.AspNetCore.Mvc;
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

            if (await IsDuplicate(city.Name))
            {
                throw new DuplicateException($"City with name {city.Name} already exists.");
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
                throw new NotFoundException($"Could not find city with id {id}.");
            }

            return city;
        }

        public async Task<City> GetByNameAsync(string name)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Name == name && e.IsActive);

            if (city == null)
            {
                throw new NotFoundException($"Could not find city with name {name}.");
            }

            return city;
        }

        public async Task<City> UpdateAsync(Guid id, CityRequest request)
        {
            City city = _converter.DtoToModel(request);
            city.Id = id;

            if (!await _context.Cities.AnyAsync(e => e.Id == city.Id))
            {
                throw new NotFoundException($"Could not find city with id {id}.");
            }

            if (await IsDuplicate(city.Name, city.Id))
            {
                throw new DuplicateException($"City with name {city.Name} already exists.");
            }

            _context.Update(city);
            await _context.SaveChangesAsync();

            return city;
        }

        public async Task<City> DeleteAsync(Guid id)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Id == id);

            if (city == null)
            {
                throw new NotFoundException($"Could not find city with id {id}.");
            }

            // Set inactive:
            city.IsActive = false;

            // Get all buildings for this city:
            List<Building> buildings = await _context.Buildings.Where(e => e.Address.CityId == id).ToListAsync();

            foreach(Building building in buildings)
            {
                // Set inactive:
                building.IsActive = false;
                _context.Update(building);
                await _context.SaveChangesAsync();

                // Get all rooms for building:
                List<Room> rooms = await _context.Rooms.Where(e => e.BuildingId == building.Id).ToListAsync();

                // Set all rooms to inactive:
                foreach (Room room in rooms)
                {
                    room.IsActive = false;
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
            }

            // Update record:
            _context.Update(city);
            await _context.SaveChangesAsync();

            return city;
        }

        private async Task<bool> IsDuplicate(string name, Guid? id=null)
        {
            if (id == null)
            {
                return await _context.Cities.AnyAsync(e => e.Name == name && e.IsActive);
            }
            else
            {
                return await _context.Cities.AnyAsync(e => e.Name == name && e.IsActive && e.Id != id);
            }
        }
    }
}
