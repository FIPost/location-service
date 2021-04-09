using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult> CreateCity(AddressRequest request)
        {
            Address address = _converter.DtoToModel(request);
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<AddressResponse>>> GetAllCities()
        {
            return Ok(_converter.ModelToDto(await _context.Addresses.ToListAsync()));
        }
    }
}
