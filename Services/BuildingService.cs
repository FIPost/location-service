using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using LocatieService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _repository;

        public BuildingService(IBuildingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Building> AddBuildingAsync(Building building)
        {
            return await _repository.AddAsync(building);
        }

        public async Task<Building> DeleteBuildingByIdAsync(Guid id)
        {
            Building building = await GetRawByIdAsync(id); 
            return await _repository.DeleteAsync(building);
        }

        public async Task<List<BuildingResponse>> GetAllBuildingsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BuildingResponse> GetBuildingByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<BuildingResponse> GetBuildingByNameAsync(string name)
        {
            return await _repository.GetByNameAsync(name);
        }

        public async Task<Building> GetRawByIdAsync(Guid id)
        {
            return await _repository.GetRawByIdAsync(id);
        }

        public async Task<bool> IsDuplicateAsync(Building building)
        {
            return await _repository.IsDuplicateAsync(building);
        }

        public async Task<Building> UpdateBuilding(Building building)
        {
            return await _repository.UpdateAsync(building);
        }
    }
}
