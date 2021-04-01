using LocatieService.Database.Contexts;
using LocatieService.Database.Converters;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocatieService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocatieController : Controller
    {
        private readonly LocatieContext _context;
        private readonly IDtoConverter<Locatie, LocatieRequest, LocatieResponse> _converter;

        public LocatieController (LocatieContext context, IDtoConverter<Locatie, LocatieRequest, LocatieResponse> converter)
        {
            _context = context;
            _converter = converter;
        }

        [HttpPost]
        public async Task<ActionResult> PostLocatie(LocatieRequest request)
        {
            Locatie locatie = _converter.DtoToModel(request);
            _context.Locaties.Add(locatie);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
