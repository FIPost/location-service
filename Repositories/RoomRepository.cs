using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly IDtoConverter<Room, RoomRequest, RoomResponse> _converter;
        private readonly IBuildingRepository _buildingRepository;
        public RoomRepository(LocatieContext locatieContext,
            IDtoConverter<Room, RoomRequest, RoomResponse> converter,
            IBuildingRepository buildingRepository)
            : base(locatieContext)
        {
            _converter = converter;
            _buildingRepository = buildingRepository;
        }

        public async Task<List<RoomResponse>> GetAllAsync()
        {
            List<Room> rooms = await GetAll().ToListAsync();
            List<RoomResponse> responses = new();

            // Add buildings:
            foreach (Room room in rooms)
            {
                RoomResponse response = _converter.ModelToDto(room);
                // Get buildingresponse using buildingRepo:
                response.Building = await _buildingRepository.GetByIdAsync(room.BuildingId);

                responses.Add(response);
            }

            return responses;
        }

        public async Task<RoomResponse> GetByIdAsync(Guid id)
        {
            Room room = await GetAll().FirstOrDefaultAsync(e => e.Id == id);

            if (room == null)
            {
                throw new Exception($"Building with id {id} not found.");
            }

            RoomResponse response = _converter.ModelToDto(room);
            // Get buildingresponse using buildingRepo:
            response.Building = await _buildingRepository.GetByIdAsync(room.BuildingId);

            return response;
        }

        public async Task<RoomResponse> GetByNameAsync(string name)
        {
            Room room = await GetAll().FirstOrDefaultAsync(e => e.Name == name);

            if (room == null)
            {
                throw new Exception($"Building with name {name} not found.");
            }

            RoomResponse response = _converter.ModelToDto(room);
            // Get buildingresponse using buildingRepo:
            response.Building = await _buildingRepository.GetByIdAsync(room.BuildingId);

            return response;
        }

        public async Task<Room> GetRawByIdAsync(Guid id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> IsDuplicateAsync(Room request)
        {
            Room room = await GetAll().FirstOrDefaultAsync(e => e.BuildingId == request.BuildingId && e.Name == request.Name);

            if (room != null)
            {
                return true;
            }

            return false;
        }
    }
}
