using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            // Get city from db
            Address address = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == request.AddressId);

            // Check if city exists
            if (address.Equals(null))
            {
                return NotFound("Opgegeven stad staat niet in het systeem.");
            }

            building.Address = address;

            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<Building>>> GetAllBuildings()
        {
            return Ok(_converter.ModelToDto(await _context.Buildings.ToListAsync()));
        }
    }
}
