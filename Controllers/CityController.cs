using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : Controller
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<City, CityRequest, CityResponse> _converter;

        public CityController(LocatieContext context, IDtoConverter<City, CityRequest, CityResponse> converter)
        {
            _context = context;
            _converter = converter;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCity(CityRequest request)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Name == request.Name);

            if (city != null)
            {
                return Conflict($"City with name {request.Name} already exists.");
            }

            City newCity = _converter.DtoToModel(request);
            _context.Cities.Add(newCity);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<CityResponse>>> GetAllCities()
        {
            return _converter.ModelToDto(await _context.Cities.ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CityResponse>> GetCityById(Guid id)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Id == id);

            if (city == null)
            {
                return NotFound($"City with id {id} not found.");
            }

            return Ok(_converter.ModelToDto(city));
        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<CityResponse>> GetCityByName(string name)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Name == name);

            if (city == null)
            {
                return NotFound($"City with name {name} not found.");
            }

            return Ok(_converter.ModelToDto(city));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteCityById(Guid id)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Id == id);

            if (city == null) // Check if city exists.
            {
                return NotFound("Object not found");
            }

            _context.Remove(city); // Remove record.
            _context.SaveChanges();

            return Ok("Successfully removed.");
        }
    }
}
