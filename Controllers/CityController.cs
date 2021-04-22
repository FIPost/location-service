using LocatieService.Database.Converters;
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
    public class CityController : Controller
    {
        private readonly ICityService _service;
        private readonly IDtoConverter<City, CityRequest, CityResponse> _converter;

        public CityController(ICityService service, IDtoConverter<City, CityRequest, CityResponse> converter)
        {
            _service = service;
            _converter = converter;
        }

        [HttpPost]
        public async Task<ActionResult<City>> CreateCity(CityRequest request)
        {
            // Check if city is a duplicate:
            City city = await _service.GetCityByNameAsync(request.Name);

            if (city != null)
            {
                return Conflict($"City with name {request.Name} already exists.");
            }

            // Convert to model and insert into db.
            City newCity = _converter.DtoToModel(request);

            return await _service.AddCityAsync(newCity);
        }

        [HttpGet]
        public async Task<ActionResult<List<City>>> GetAllCities()
        {
            return await _service.GetAllCitiesAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<City>> GetCityById(Guid id)
        {
            return await _service.GetCityByIdAsync(id);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<City>> GetCityByName(string name)
        {
            return await _service.GetCityByNameAsync(name);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<City>> DeleteCityById(Guid id)
        {
            // Get city object first:
            City city = await _service.GetCityByIdAsync(id);

            if (city == null)
            {
                return NotFound($"Entity with id {id} not found.");
            }

            return await _service.DeleteCityAsync(city);
        }
    }
}
