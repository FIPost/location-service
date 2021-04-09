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
    public class RoomController : Controller
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<Room, RoomRequest, RoomResponse> _converter;

        public RoomController(LocatieContext context, IDtoConverter<Room, RoomRequest, RoomResponse> converter)
        {
            _context = context;
            _converter = converter;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoom(RoomRequest request)
        {
            Room Room = _converter.DtoToModel(request);
            // Get city from db
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == request.BuildingId);

            // Check if city exists
            if (building.Equals(null))
            {
                return NotFound("Opgegeven stad staat niet in het systeem.");
            }

            Room.Building = building;

            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAllRooms()
        {
            return Ok(_converter.ModelToDto(await _context.Rooms.ToListAsync()));
        }
    }
}
