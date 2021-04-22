using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Repositories
{
    public class BuildingRepository : Repository<Building>, IBuildingRepository
    {
        private readonly IDtoConverter<Building, BuildingRequest, BuildingResponse> _converter;
        public BuildingRepository(LocatieContext locatieContext, IDtoConverter<Building, BuildingRequest, BuildingResponse> converter)
            : base(locatieContext)
        {
            _converter = converter;
        }

        public async Task<List<BuildingResponse>> GetAllAsync()
        {
            List<Building> buildings = await GetAll().ToListAsync();
            List<BuildingResponse> responses = new();

            // Add cities:
            foreach (Building building in buildings)
            {
                BuildingResponse response = _converter.ModelToDto(building);
                response.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId);

                responses.Add(response);
            }

            return responses;
        }

        public async Task<BuildingResponse> GetByIdAsync(Guid id)
        {
            Building building = await GetAll().FirstOrDefaultAsync(e => e.Id == id);

            if (building == null)
            {
                throw new Exception($"Building with name {id} not found.");
            }

            BuildingResponse response = _converter.ModelToDto(building);
            response.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId);

            return response;
        }

        public async Task<BuildingResponse> GetByNameAsync(string name)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Name == name);

            if (building == null)
            {
                throw new Exception($"Building with name {name} not found.");
            }

            BuildingResponse response = _converter.ModelToDto(building);
            response.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId);

            return response;
        }

        public async Task<Building> GetRawByIdAsync(Guid id)
        {
            return await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> IsDuplicateAsync(Building request)
        {
            Building building = await GetAll().FirstOrDefaultAsync(
                e => e.Name == request.Name
                && e.Address.PostalCode == request.Address.PostalCode
                && e.Address.Street == request.Address.Street);

            if (building != null)
            {
                return true;
            }

            return false;
        }
    }
}
