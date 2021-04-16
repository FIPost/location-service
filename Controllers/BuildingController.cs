using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : Controller
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<Building, BuildingRequest, BuildingResponse> _converter;

        public BuildingController(LocatieContext context, IDtoConverter<Building, BuildingRequest, BuildingResponse> converter)
        {
            _context = context;
            _converter = converter;
        }

        [HttpPost]
        public async Task<ActionResult> CreateBuilding(BuildingRequest request)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(
                e => e.Name == request.Name
                && e.Address.PostalCode == request.Address.PostalCode
                && e.Address.Street == request.Address.Street);

            if(building != null)
            {
                return Conflict($"Building with name {request.Name} at this address already exists.");
            }

            Building newBuilding = _converter.DtoToModel(request);
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Id == request.Address.CityId);

            if (city == null)
            {
                return BadRequest($"City with id {request.Address.CityId} does not exist.");
            }

            _context.Buildings.Add(newBuilding);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<BuildingResponse>>> GetAllBuildings()
        {
            List<Building> buildings = await _context.Buildings.ToListAsync();
            List<BuildingResponse> responses = new();

            foreach (Building building in buildings)
            {
                BuildingResponse response = _converter.ModelToDto(building);
                response.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId);

                responses.Add(response);
            }

            return Ok(responses);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<BuildingResponse>> GetBuildingById(Guid id)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id);

            if (building == null)
            {
                return NotFound($"Building with id {id} not found.");
            }

            BuildingResponse response = _converter.ModelToDto(building);
            response.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId);

            return Ok(response);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<BuildingResponse>> GetBuildingByName(string name)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Name == name);

            if (building == null)
            {
                return NotFound($"Building with name {name} not found.");
            }

            BuildingResponse response = _converter.ModelToDto(building);
            response.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteBuildingById(Guid id)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id);

            if (building == null) // Check if building exists.
            {
                return NotFound("Object not found");
            }

            _context.Remove(building); // Remove record.
            _context.SaveChanges();

            return Ok("Successfully removed.");
        }
    }
}
