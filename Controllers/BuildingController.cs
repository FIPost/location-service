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

            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<BuildingResponse>>> GetAllBuildings()
        {
            return Ok(_converter.ModelToDto(await _context.Buildings.ToListAsync()));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<BuildingResponse>> GetBuildingById(Guid id)
        {
            try
            {
                return _converter.ModelToDto(await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id));
            }
            catch (NullReferenceException)
            {
                return NotFound("Object not found");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteBuildingById(Guid id)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == id);

            if (building == null) // Check if address exists.
            {
                return NotFound("Object not found");
            }

            _context.Remove(building.Address); // Remove reference to this building in address table.
            _context.Buildings.Remove(building); // Remove record.
            _context.SaveChanges();

            return Ok("Successfully removed.");
        }
    }
}
