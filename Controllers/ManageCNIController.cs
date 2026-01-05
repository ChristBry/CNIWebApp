using CNIWebApp.Data;
using CNIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CNIWebApp.Controllers
{
    public class ManageCNIController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ManageCNIController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult TesterMaBase()
        {
            try
            {
                // 1. Vérifie si le contexte n'est pas nul
                if (_context == null) return Content("Le contexte de base de données est nul !");

                // 2. Tente d'ouvrir la connexion
                bool canConnect = _context.Database.CanConnect();

                if (canConnect)
                {
                    return Content($"CONNEXION RÉUSSIE ! \nBase de données : {_context.Database.GetDbConnection().Database}");
                }
                else
                {
                    return Content("ÉCHEC : Impossible de se connecter à MySQL. Vérifiez WampServer.");
                }
            }
            catch (Exception ex)
            {
                return Content($"ERREUR CRITIQUE : {ex.Message} \n\nDétails : {ex.InnerException?.Message}");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Génération du numéro CNI (10 chiffres aléatoires)
            Random res = new Random();
            string codeCni = "";
            for (int i = 0; i < 10; i++)
            {
                codeCni += res.Next(0, 10).ToString();
            }

            // Génération de l'identifiant (2026 + reste pour atteindre 15 caractères)
            string prefix = "2026";
            string spsm = "0000000";
            string idUnique = prefix;
            for (int i = 0; i < 11; i++)
            {
                idUnique += res.Next(0, 10).ToString();
            }

            // Préparation de l'objet avec les dates
            var model = new Cni
            {
                Num_cni = codeCni,
                Num_identifiant = idUnique,
                Date_naissance = DateTime.Now,
                Date_delivrance = DateTime.Now,
                Date_expiration = DateTime.Now.AddYears(10),
                spsm = spsm
            };

            return View(model);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cni cni, IFormFile photoFile)
        {
            // 1. Vérification MANUELLE de la photo avant tout
            if (photoFile == null || photoFile.Length == 0)
            {
                ModelState.AddModelError("Image", "La photo est strictement obligatoire.");
            }
            else
            {
                // Si la photo est là, on retire l'erreur automatique du ModelState
                ModelState.Remove("Image");
            }
            ModelState.Remove("Taille");
            if (ModelState.IsValid)
            {
                // 1. GESTION DE LA PHOTO
                if (photoFile != null && photoFile.Length > 0)
                {
                    string folder = "images/photos/";
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + photoFile.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    string filePath = Path.Combine(serverFolder, uniqueFileName);

                    // Enregistrer le fichier physiquement
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(fileStream);
                    }

                    cni.Image = "/" + folder + uniqueFileName;
                }

                // 2. SAUVEGARDE EN BASE DE DONNÉES
                _context.Add(cni);
                await _context.SaveChangesAsync();

                // Rediriger vers une page de succès ou la liste
                return RedirectToAction("Index", "Home");
            }

            var message = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            System.Diagnostics.Debug.WriteLine("ÉCHEC DE VALIDATION : " + message);

            // On renvoie la vue. L'erreur sera affichée grâce au ValidationSummary de la vue
            return View(cni);
        }

        public async Task<IActionResult> Index()
        {
            // Récupère la liste triée par date de création (du plus récent au plus ancien)
            var cnis = await _context.cni_card.OrderByDescending(c => c.Id).ToListAsync();
            return View(cnis);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cni = await _context.cni_card.FindAsync(id);
            if (cni == null) return NotFound();

            return View(cni);
        }

        // POST: ManageCNI/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cni cni, IFormFile? photoFile)
        {
            if (id != cni.Id) return NotFound();

            // On retire la validation pour les champs qui posent problème
            ModelState.Remove("Image");
            ModelState.Remove("Taille");

            if (ModelState.IsValid)
            {
                try
                {
                    if (photoFile != null && photoFile.Length > 0)
                    {
                        // 1. Supprimer l'ancienne photo physique si elle existe
                        if (!string.IsNullOrEmpty(cni.Image))
                        {
                            var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, cni.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }

                        // 2. Enregistrer la nouvelle photo
                        string folder = "images/photos/";
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + photoFile.FileName;
                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                        string filePath = Path.Combine(serverFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await photoFile.CopyToAsync(fileStream);
                        }
                        cni.Image = "/" + folder + uniqueFileName;
                    }

                    _context.Update(cni);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.cni_card.Any(e => e.Id == cni.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cni);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cni = await _context.cni_card.FirstOrDefaultAsync(m => m.Id == id);
            if (cni == null) return NotFound();

            return View(cni);
        }

        // POST: ManageCNI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cni = await _context.cni_card.FindAsync(id);
            if (cni != null)
            {
                // Supprimer la photo physiquement du dossier wwwroot
                if (!string.IsNullOrEmpty(cni.Image))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, cni.Image.TrimStart('/'));
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                }

                _context.cni_card.Remove(cni);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
