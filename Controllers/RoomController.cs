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

            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomResponse>>> GetAllRooms()
        {
            return Ok(_converter.ModelToDto(await _context.Rooms.ToListAsync()));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<RoomResponse>> GetRoomById(Guid id)
        {
            try
            {
                return _converter.ModelToDto(await _context.Rooms.FirstOrDefaultAsync(e => e.Id == id));
            }
            catch (NullReferenceException)
            {
                return NotFound("Object not found");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteRoomById(Guid id)
        {
            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == id);

            if (room == null) // Check if address exists.
            {
                return NotFound("Object not found");
            }

            _context.Remove(room.Building); // Remove reference to this room in building table.
            _context.Remove(room); // Remove record.
            _context.SaveChanges();

            return Ok("Successfully removed.");
        }
    }
}
