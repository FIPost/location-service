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
            List<Institution> institutions = await _context.Institutions.ToListAsync();
            List<InstitutionResponse> responses = new();

            foreach (Institution institution in institutions)
            {
                responses.Add(_converter.ModelToDto(institution)); // Add response to list.
            }

            return Ok(responses);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<InstitutionResponse>> GetInstitutionById(Guid id)
        {
            Institution institution = await _context.Institutions.FirstOrDefaultAsync(e => e.Id == id);
            InstitutionResponse response = _converter.ModelToDto(institution);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteInstitutionById(Guid id)
        {
            Institution institution = await _context.Institutions.FirstOrDefaultAsync(e => e.Id == id); // Get institution

            if (institution == null) // Check if institution exists.
            {
                return NotFound("Object not found");
            }

            _context.Remove(institution); // Remove record.
            _context.SaveChanges();

            return Ok("Successfully removed.");
        }
    }
}
