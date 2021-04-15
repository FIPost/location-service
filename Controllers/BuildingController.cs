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
            Building building = _converter.DtoToModel(request);

            Address address = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == request.AddressId);

            // Check if address exists.
            if (address == null)
            {
                return BadRequest("This address does not exist");
            }

            _context.Buildings.Add(building);
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
                response.Address = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == building.AddressId);
                responses.Add(response);
            }

            return Ok(_converter.ModelToDto(await _context.Buildings.ToListAsync()));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<BuildingResponse>> GetBuildingById(Guid id)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id);
            BuildingResponse response = _converter.ModelToDto(building);
            response.Address = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == building.AddressId); // Insert address to model.

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
