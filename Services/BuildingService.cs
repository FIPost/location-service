using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<Building, BuildingRequest, BuildingResponse> _converter;

        public BuildingService(LocatieContext context, IDtoConverter<Building, BuildingRequest, BuildingResponse> converter)
        {
            _context = context;
            _converter = converter;
        }


        public async Task<BuildingResponse> AddAsync(BuildingRequest request)
        {
            Building building = _converter.DtoToModel(request);

            if (await IsDuplicateAsync(building))
            {
                throw new Exception("This building already exists.");
            }

            await _context.AddAsync(building);
            await _context.SaveChangesAsync();

            return await CreateResponseAsync(building);
        }

        public async Task<List<BuildingResponse>> GetAllAsync()
        {
            List<Building> buildings = await _context.Buildings.ToListAsync();
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
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id);

            if (building == null)
            {
                throw new Exception($"Building with id {id} not found.");
            }

            return await CreateResponseAsync(building);
        }

        public async Task<Building> GetRawByIdAsync(Guid id)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id);
            
            if (building == null)
            {
                throw new Exception($"Building with id {id} not found.");
            }

            return building;
        }

        public async Task<BuildingResponse> GetByNameAsync(string name)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Name == name);

            if (building == null)
            {
                throw new Exception($"Building with name {name} not found.");
            }

            return await CreateResponseAsync(building);
        }
        public async Task<BuildingResponse> UpdateAsync(Guid id, BuildingRequest request)
        {
            Building building = _converter.DtoToModel(request);
            building.Id = id;

            _context.Update(building);
            await _context.SaveChangesAsync();

            return await CreateResponseAsync(building);
        }

        public async Task DeleteAsync(Guid id)
        {
            // Set inactive:
            Building building = await GetRawByIdAsync(id);
            building.Id = id; // Id is needed for updating record.
            building.IsActive = false;

            // Update record:
            _context.Update(building);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsDuplicateAsync(Building building)
        {
            Building duplicate = await _context.Buildings.FirstOrDefaultAsync(
                e => e.Name == building.Name
                && e.Address.PostalCode == building.Address.PostalCode
                && e.Address.Street == building.Address.Street);

            if (duplicate != null)
            {
                return true;
            }

            return false;
        }

        private async Task<BuildingResponse> CreateResponseAsync(Building building)
        {
            BuildingResponse response = _converter.ModelToDto(building);
            response.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId);

            return response;
        }
    }
}
