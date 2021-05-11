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
    public class RoomService : Controller, IRoomService
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<Room, RoomRequest, RoomResponse> _converter;
        private readonly IDtoConverter<Building, BuildingRequest, BuildingResponse> _buildingConverter;

        public RoomService(LocatieContext context,
            IDtoConverter<Room, RoomRequest, RoomResponse> converter,
            IDtoConverter<Building, BuildingRequest, BuildingResponse> buildingConverter)
        {
            _context = context;
            _converter = converter;
            _buildingConverter = buildingConverter;
        }

        public async Task<ActionResult<RoomResponse>> AddAsync(RoomRequest request)
        {
            Room room = _converter.DtoToModel(request);

            if (await IsDuplicateAsync(room))
            {
                return Conflict("This room already exists.");
            }

            await _context.AddAsync(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction("AddRoom", await CreateResponseAsync(room));
        }

        public async Task<ActionResult<List<RoomResponse>>> GetAllAsync()
        {
            List<Room> rooms = await _context.Rooms.Where(e => e.IsActive).ToListAsync();
            List<RoomResponse> responses = new();

            // Add buildings:
            foreach (Room room in rooms)
            {
                RoomResponse response = _converter.ModelToDto(room);
                Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == room.BuildingId); // Get building model.
                BuildingResponse buildingResponse = _buildingConverter.ModelToDto(building); // Insert building to model.
                buildingResponse.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId); // Insert city into address.
                response.Building = buildingResponse;

                responses.Add(response);
            }

            return Ok(responses);
        }

        public async Task<ActionResult<RoomResponse>> GetByIdAsync(Guid id)
        {
            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == id);

            if (room == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            return Ok(await CreateResponseAsync(room));
        }

        public async Task<ActionResult<RoomResponse>> GetByNameAsync(string name)
        {
            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.Name == name);

            if (room == null)
            {
                return NotFound($"Room with name {name} not found.");
            }

            return Ok(await CreateResponseAsync(room));
        }

        public async Task<ActionResult<RoomResponse>> UpdateAsync(Guid id, RoomRequest request)
        {
            Room room = _converter.DtoToModel(request);
            room.Id = id;

            _context.Update(room);
            await _context.SaveChangesAsync();

            return Ok(await CreateResponseAsync(room));
        }

        public async Task<ActionResult> DeleteRoomAsync(Guid id)
        {
            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == id);

            if (room == null)
            {
                return NotFound($"Room with id {id} not found.");
            }

            // Set inactive:
            room.IsActive = false;

            // Update record:
            _context.Update(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> IsDuplicateAsync(Room room)
        {
            Room duplicate = await _context.Rooms.FirstOrDefaultAsync(e => e.BuildingId == room.BuildingId
                && e.Name == room.Name
                && e.IsActive);

            if (duplicate != null)
            {
                return true;
            }

            return false;
        }

        private async Task<RoomResponse> CreateResponseAsync(Room room)
        {
            RoomResponse response = _converter.ModelToDto(room);
            Building building = await _context.Buildings.FirstOrDefaultAsync(e => e.Id == room.BuildingId); // Get building model.
            BuildingResponse buildingResponse = _buildingConverter.ModelToDto(building); // Insert building to model.
            buildingResponse.Address.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == building.Address.CityId); // Insert city into address.
            response.Building = buildingResponse;

            return response;
        }
    }
}
