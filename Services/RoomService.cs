using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using LocatieService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repository;

        public RoomService(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<Room> AddRoomAsync(Room room)
        {
            return await _repository.AddAsync(room);
        }

        public async Task<Room> DeleteRoomAsync(Room room)
        {
            return await _repository.DeleteAsync(room);
        }

        public async Task<List<RoomResponse>> GetAllRoomsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Room> GetRawByIdAsync(Guid id)
        {
            return await _repository.GetRawByIdAsync(id);
        }

        public async Task<RoomResponse> GetRoomByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<RoomResponse> GetRoomByNameAsync(string name)
        {
            return await _repository.GetByNameAsync(name);
        }

        public async Task<bool> IsDuplicateAsync(Room room)
        {
            return await _repository.IsDuplicateAsync(room);
        }

        public async Task<Room> UpdateRoom(Room room)
        {
            return await _repository.UpdateAsync(room);
        }
    }
}
