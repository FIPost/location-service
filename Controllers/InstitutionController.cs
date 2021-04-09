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
        public async Task<ActionResult> CreateInstitute(InstitutionRequest request)
        {
            Institution institution = _converter.DtoToModel(request);
            _context.Institutions.Add(institution);
            await _context.SaveChangesAsync();

            return Created("Created", request);
        }

        [HttpGet]
        public async Task<ActionResult<List<InstitutionResponse>>> GetAllInstitutes()
        {
            return _converter.ModelToDto(await _context.Institutions.ToListAsync());
        }
    }
}
