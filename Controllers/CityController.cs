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

        public CityController(ICityService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<City>> AddCity(CityRequest request)
        {
            return Ok(await _service.AddAsync(request));
        }

        [HttpGet]
        public async Task<ActionResult<List<City>>> GetAllCities()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<City>> GetCityById(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<City>> GetCityByName(string name)
        {
            return await _service.GetByNameAsync(name);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<City>> UpdateCity(Guid id, CityRequest request)
        {
            return await _service.UpdateAsync(id, request);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<City>> DeleteCityById(Guid id)
        {
            await _service.DeleteAsync(id);

            return Ok();
        }
    }
}
