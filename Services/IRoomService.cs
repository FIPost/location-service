using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public interface IRoomService
    {
        Task<Room> AddRoomAsync(Room room);
        Task<List<RoomResponse>> GetAllRoomsAsync();
        Task<RoomResponse> GetRoomByIdAsync(Guid id);
        Task<Room> GetRawByIdAsync(Guid id);
        Task<RoomResponse> GetRoomByNameAsync(string name);
        Task<Room> UpdateRoom(Room room);
        Task<Room> DeleteRoomAsync(Room room);
        Task<bool> IsDuplicateAsync(Room room);
    }
}
