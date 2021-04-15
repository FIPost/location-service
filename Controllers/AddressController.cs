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
    public class AddressController : Controller
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<Address, AddressRequest, AddressResponse> _converter;

        public AddressController(LocatieContext context, IDtoConverter<Address, AddressRequest, AddressResponse> converter)
        {
            _context = context;
            _converter = converter;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAddress(AddressRequest request)
        {
            Address address = _converter.DtoToModel(request);

            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Id == request.CityId);

            // Check if city exists.
            if (city == null)
            {
                return BadRequest("This city does not exist");
            }

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<AddressResponse>>> GetAllAddresses()
        {
            List<Address> addresses = await _context.Addresses.ToListAsync();
            List<AddressResponse> responses = new();

            foreach (Address address in addresses)
            {
                AddressResponse response = _converter.ModelToDto(address);
                response.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == address.CityId);
                responses.Add(response);
            }

            return Ok(responses);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AddressResponse>> GetAddressById(Guid id)
        {
            Address address = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == id);
            AddressResponse response = _converter.ModelToDto(address);
            response.City = await _context.Cities.FirstOrDefaultAsync(e => e.Id == address.CityId);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAddressById(Guid id)
        {
            Address address = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == id);

            if (address == null) // Check if address exists.
            {
                return NotFound("Object not found");
            }

            _context.Remove(address); // Remove record.
            _context.SaveChanges();

            return Ok("Successfully removed.");
        }
    }
}
