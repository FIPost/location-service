using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Services
{
    public interface IBuildingService
    {
        Task<ActionResult<BuildingResponse>> AddAsync(BuildingRequest request);
        Task<ActionResult<List<BuildingResponse>>> GetAllAsync();
        Task<ActionResult<BuildingResponse>> GetByIdAsync(Guid id);
        Task<ActionResult<BuildingResponse>> GetByNameAsync(string name);
        Task<ActionResult<BuildingResponse>> UpdateAsync(Guid id, BuildingRequest request);
        Task<ActionResult> DeleteAsync(Guid id);
    }
}
