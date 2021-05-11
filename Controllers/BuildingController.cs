using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using LocatieService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : Controller
    {
        private readonly IBuildingService _service;

        public BuildingController(IBuildingService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<BuildingResponse>> AddBuilding(BuildingRequest request)
        {
            return await _service.AddAsync(request);
        }

        [HttpGet]
        public async Task<ActionResult<List<BuildingResponse>>> GetAllBuildings()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<BuildingResponse>> GetBuildingById(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<BuildingResponse>> GetBuildingByName(string name)
        {
            return await _service.GetByNameAsync(name);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<BuildingResponse>> UpdateBuilding(Guid id, BuildingRequest request)
        {
            return await _service.UpdateAsync(id, request);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Building>> DeleteBuildingById(Guid id)
        {
            return await _service.DeleteAsync(id);
        }
    }
}
