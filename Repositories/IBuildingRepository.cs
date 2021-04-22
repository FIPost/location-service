using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Repositories
{
    public interface IBuildingRepository : IRepository<Building>
    {
        Task<BuildingResponse> GetByIdAsync(Guid id);
        Task<Building> GetRawByIdAsync(Guid id);
        Task<BuildingResponse> GetByNameAsync(string name);
        Task<List<BuildingResponse>> GetAllAsync();
        Task<bool> IsDuplicateAsync(Building building);
    }
}
