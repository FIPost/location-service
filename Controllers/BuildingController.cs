using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using LocatieService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICityService _cityService;
        private readonly IDtoConverter<Building, BuildingRequest, BuildingResponse> _converter;

        public BuildingController(IBuildingService service, ICityService cityService, IDtoConverter<Building, BuildingRequest, BuildingResponse> converter)
        {
            _service = service;
            _converter = converter;
            _cityService = cityService;
        }

        [HttpPost]
        public async Task<ActionResult<Building>> CreateBuilding(BuildingRequest request)
        {
            // Check if building already exists.
            if (await _service.IsDuplicateAsync(_converter.DtoToModel(request)))
            {
                return Conflict($"Building with name {request.Name} at this address already exists.");
            }

            Building building = _converter.DtoToModel(request);
            City city = await _cityService.GetCityByIdAsync(request.Address.CityId);

            if (city == null)
            {
                return BadRequest($"City with id {request.Address.CityId} does not exist.");
            }

            return await _service.AddBuildingAsync(building);
        }

        [HttpGet]
        public async Task<ActionResult<List<BuildingResponse>>> GetAllBuildings()
        {
            return await _service.GetAllBuildingsAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<BuildingResponse>> GetBuildingById(Guid id)
        {
            return await _service.GetBuildingByIdAsync(id);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<BuildingResponse>> GetBuildingByName(string name)
        {
            return await _service.GetBuildingByNameAsync(name);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Building>> DeleteBuildingById(Guid id)
        {
            return await _service.DeleteBuildingByIdAsync(id);
        }
    }
}
