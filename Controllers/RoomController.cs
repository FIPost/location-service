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
        private readonly IDtoConverter<Building, BuildingRequest, BuildingResponse> _buildingConverter;

        public RoomController(LocatieContext context,
            IDtoConverter<Room, RoomRequest, RoomResponse> converter,
            IDtoConverter<Building, BuildingRequest, BuildingResponse> buildingConverter)
        {
            _context = context;
            _converter = converter;
            _buildingConverter = buildingConverter;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoom(RoomRequest request)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == request.BuildingId);

            // Check if building exists.
            if (building == null)
            {
                return BadRequest("This building does not exist");
            }

            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.BuildingId == request.BuildingId && e.Name == request.Name);

            if (room != null)
            {
                return Conflict($"City with name {request.Name} and buildingId {request.BuildingId} already exists.");
            }

            Room newRoom = _converter.DtoToModel(request);

            _context.Rooms.Add(newRoom);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomResponse>>> GetAllRooms()
        {
            List<Room> rooms = await _context.Rooms.ToListAsync();
            List<RoomResponse> responses = new();

            foreach (Room room in rooms)
            {
                RoomResponse response = _converter.ModelToDto(room);
                response.Building = await GetBuilding(room.BuildingId);
                responses.Add(response);
            }

            return Ok(responses);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<RoomResponse>> GetRoomById(Guid id)
        {
            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == id);

            if (room == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            RoomResponse response = _converter.ModelToDto(room);
            response.Building = await GetBuilding(room.BuildingId);

            return Ok(response);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<RoomResponse>> GetRoomByName(string name)
        {
            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.Name == name);

            if (room == null)
            {
                return NotFound($"Room with name {name} not found.");
            }

            RoomResponse response = _converter.ModelToDto(room);
            response.Building = await GetBuilding(room.BuildingId);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteRoomById(Guid id)
        {
            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == id);

            if (room == null) // Check if room exists.
            {
                return NotFound("Object not found");
            }

            _context.Remove(room); // Remove record.
            _context.SaveChanges();

            return Ok("Successfully removed.");
        }

        private async Task<BuildingResponse> GetBuilding(Guid buildingId)
        {
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == buildingId); // Get building model.
            BuildingResponse buildingResponse = _buildingConverter.ModelToDto(building); // Insert building to model.
            buildingResponse.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId); // Insert city into address.

            return buildingResponse;
        }
    }
}
