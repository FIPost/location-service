using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public interface IBuildingService
    {
        Task<Building> AddBuildingAsync(Building building);
        Task<List<BuildingResponse>> GetAllBuildingsAsync();
        Task<BuildingResponse> GetBuildingByIdAsync(Guid id);
        Task<Building> GetRawByIdAsync(Guid id);
        Task<BuildingResponse> GetBuildingByNameAsync(string name);
        Task<Building> UpdateBuilding(Building building);
        Task<Building> DeleteBuildingByIdAsync(Guid id);
        Task<bool> IsDuplicateAsync(Building building);
    }
}
