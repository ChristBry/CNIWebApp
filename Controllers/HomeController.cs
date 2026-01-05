using CNIWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNIWebApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult TestConnexion()
        {
            try
            {
                var count = _context.cni_card.Count();
                return Content($"Succès ! Connexion établie. Nombre de CNI en base : {count}");
            }
            catch (Exception ex)
            {
                // Si ça échoue, on affiche l'erreur précise
                return Content($"Échec de connexion : {ex.Message}");
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        // Optionnel : Une action pour gérer les erreurs
        public IActionResult Error()
        {
            return View();
        }
    }
}
