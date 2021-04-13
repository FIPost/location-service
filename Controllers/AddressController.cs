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
            // Get city from db
            City city = await _context.Cities.FirstOrDefaultAsync(e => e.Id == request.CityId);

            // Check if city exists
            if (city.Equals(null))
            {
                return NotFound("Opgegeven stad staat niet in het systeem.");
            }

            address.City = city;

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<AddressResponse>>> GetAllAddresses()
        {
            return Ok(_converter.ModelToDto(await _context.Addresses.ToListAsync()));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AddressResponse>> GetAddressById(Guid id)
        {
            try
            {
                return _converter.ModelToDto(await _context.Addresses.FirstOrDefaultAsync(e => e.Id == id));
            }
            catch (NullReferenceException)
            {
                return NotFound("Object not found");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<AddressResponse>> DeleteAddressById(Guid id)
        {
            Address address = await _context.Addresses.FirstOrDefaultAsync(e => e.Id == id);

            if (address.Equals(null)) // Check if address exists.
            {
                return NotFound("Object not found");
            }

            _context.Addresses.Remove(address); // Remove record.
            _context.SaveChanges();

            return Ok("Successfully removed.");
        }
    }
}
