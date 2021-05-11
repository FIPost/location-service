using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public class BuildingService : Controller, IBuildingService
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<Building, BuildingRequest, BuildingResponse> _converter;

        public BuildingService(LocatieContext context, IDtoConverter<Building, BuildingRequest, BuildingResponse> converter)
        {
            _context = context;
            _converter = converter;
        }


        public async Task<ActionResult<BuildingResponse>> AddAsync(BuildingRequest request)
        {
            Building building = _converter.DtoToModel(request);

            if (await IsDuplicateAsync(building))
            {
                return Conflict("This building already exists.");
            }

            await _context.AddAsync(building);
            await _context.SaveChangesAsync();

            return CreatedAtAction("AddBuilding", await CreateResponseAsync(building));
        }

        public async Task<ActionResult<List<BuildingResponse>>> GetAllAsync()
        {
            List<Building> buildings = await _context.Buildings.Where(e => e.IsActive).ToListAsync();
            List<BuildingResponse> responses = new();

            // Add cities:
            foreach (Building building in buildings)
            {
                BuildingResponse response = _converter.ModelToDto(building);
                response.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId);

                responses.Add(response);
            }

            return Ok(responses);
        }

        public async Task<ActionResult<BuildingResponse>> GetByIdAsync(Guid id)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id);

            if (building == null)
            {
                return NotFound($"Building with id {id} not found.");
            }

            return Ok(await CreateResponseAsync(building));
        }

        public async Task<ActionResult<BuildingResponse>> GetByNameAsync(string name)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Name == name);

            if (building == null)
            {
                return NotFound($"Building with name {name} not found.");
            }

            return Ok(await CreateResponseAsync(building));
        }
        public async Task<ActionResult<BuildingResponse>> UpdateAsync(Guid id, BuildingRequest request)
        {
            Building building = _converter.DtoToModel(request);
            building.Id = id;

            _context.Update(building);
            await _context.SaveChangesAsync();

            return Ok(await CreateResponseAsync(building));
        }

        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id);

            if (building == null)
            {
                return NotFound($"Building with id {id} not found.");
            }

            // Set inactive:
            building.IsActive = false;

            // Get all rooms from this building:
            List<Room> rooms = await _context.Rooms.Where(e => e.BuildingId == id).ToListAsync();

            // Set all rooms to inactive:
            foreach (Room room in rooms)
            {
                room.IsActive = false;
                // Update record:
                _context.Update(room);
                await _context.SaveChangesAsync();
            }

            // Update record:
            _context.Update(building);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> IsDuplicateAsync(Building building)
        {
            Building duplicate = await _context.Buildings.FirstOrDefaultAsync(
                e => e.Name == building.Name
                && e.Address.PostalCode == building.Address.PostalCode
                && e.Address.Street == building.Address.Street
                && e.IsActive);

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
