using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNIWebApp.Data;
using CNIWebApp.Models;

namespace CNIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CniApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CniApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CniApi/ListeNoms
        [HttpGet("ListeNoms")]
        public async Task<ActionResult<IEnumerable<InterfaceCNI>>> GetNomsCitoyens()
        {
            try
            {
                var citoyens = await _context.cni_card.ToListAsync();

                var result = citoyens.Select(c => new ApiCni
                {
                    Nom = c.Nom,
                    Prenom = c.Prenom,
                    Num_cni = c.Num_cni
                })
                .SelectMany(api => api.ListInfo())
                .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne : {ex.Message}");
            }
        }
    }
}
