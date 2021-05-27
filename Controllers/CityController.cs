using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using LocatieService.helpers;
using LocatieService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Controllers
{
    [ApiController]
    [Route("api/cities")]
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
            try
            {
                return Ok(await _service.AddAsync(request));
            }
            catch (DuplicateException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<City>>> GetAllCities()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<City>> GetCityById(Guid id)
        {
            try
            {
                return Ok(await _service.GetByIdAsync(id));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<City>> GetCityByName(string name)
        {
            try
            {
                return Ok(await _service.GetByNameAsync(name));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<City>> UpdateCity(Guid id, CityRequest request)
        {
            try
            {
                return Ok(await _service.UpdateAsync(id, request));
            }
            catch (DuplicateException e)
            {
                return Conflict(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<City>> DeleteCityById(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        [Route("health")]
        public ActionResult Health()
        {
            return Ok();
        }
    }
}
