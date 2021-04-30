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
    public class RoomService : IRoomService
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

        public async Task<RoomResponse> AddAsync(RoomRequest request)
        {
            Room room = _converter.DtoToModel(request);

            if (await IsDuplicateAsync(room))
            {
                throw new Exception("This room already exists.");
            }

            await _context.AddAsync(room);
            await _context.SaveChangesAsync();

            return await CreateResponseAsync(room);
        }

        public async Task<List<RoomResponse>> GetAllAsync()
        {
            List<Room> rooms = await _context.Rooms.ToListAsync();
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

            return responses;
        }

        public async Task<RoomResponse> GetByIdAsync(Guid id)
        {
            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.Id == id);

            if (room == null)
            {
                throw new Exception($"Building with id {id} not found.");
            }

            return await CreateResponseAsync(room);
        }

        public async Task<Room> GetRawByIdAsync(Guid id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<RoomResponse> GetByNameAsync(string name)
        {
            Room room = await _context.Rooms.FirstOrDefaultAsync(e => e.Name == name);

            if (room == null)
            {
                throw new Exception($"Building with name {name} not found.");
            }

            return await CreateResponseAsync(room);
        }

        public async Task<RoomResponse> UpdateAsync(Guid id, RoomRequest request)
        {
            Room room = _converter.DtoToModel(request);
            room.Id = id;

            _context.Update(room);
            await _context.SaveChangesAsync();

            return await CreateResponseAsync(room);
        }

        public async Task DeleteRoomAsync(Guid id)
        {
            // Set inactive:
            Room room = await GetRawByIdAsync(id);
            room.Id = id; // Id is needed for updating record.
            room.IsActive = false;

            // Update record:
            _context.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsDuplicateAsync(Room room)
        {
            Room duplicate = await _context.Rooms.FirstOrDefaultAsync(e => e.BuildingId == room.BuildingId && e.Name == room.Name);

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
