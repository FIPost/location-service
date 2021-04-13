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
    public class InstitutionController : Controller
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<Institution, InstitutionRequest, InstitutionResponse> _converter;

        public InstitutionController(LocatieContext context, IDtoConverter<Institution, InstitutionRequest, InstitutionResponse> converter)
        {
            _context = context;
            _converter = converter;
        }

        [HttpPost]
        public async Task<ActionResult> CreateInstitution(InstitutionRequest request)
        {
            Institution institution = _converter.DtoToModel(request);

            _context.Institutions.Add(institution);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<InstitutionResponse>>> GetAllInstitutions()
        {
            return _converter.ModelToDto(await _context.Institutions.ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<InstitutionResponse>> GetInstitutionById(Guid id)
        {
            try
            {
                return _converter.ModelToDto(await _context.Institutions.FirstOrDefaultAsync(e => e.Id == id));
            }
            catch (NullReferenceException)
            {
                return NotFound("Object not found");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteInstitutionById(Guid id)
        {
            Institution institution = await _context.Institutions.FirstOrDefaultAsync(e => e.Id == id); // Get institution

            if (institution == null) // Check if address exists.
            {
                return NotFound("Object not found");
            }

            foreach (Address address in institution.Addresses)
            {
                _context.Remove(address); // Remove all references to this institution in address table.
            }

            _context.Remove(institution); // Remove record.
            _context.SaveChanges();

            return Ok("Successfully removed.");
        }
    }
}
