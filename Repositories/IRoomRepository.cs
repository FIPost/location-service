using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<RoomResponse> GetByIdAsync(Guid id);
        Task<Room> GetRawByIdAsync(Guid id);
        Task<RoomResponse> GetByNameAsync(string name);
        Task<List<RoomResponse>> GetAllAsync();
        Task<bool> IsDuplicateAsync(Room room);
    }
}
