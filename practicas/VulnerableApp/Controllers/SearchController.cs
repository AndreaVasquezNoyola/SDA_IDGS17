using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableApp.Data;
using VulnerableApp.Models;

namespace VulnerableApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<SearchController> _logger;

        public SearchController(AppDbContext db, ILogger<SearchController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public ActionResult Index(string search)
        {
            _logger.LogInformation("Inicio Search.Index con parámetro: {search}", search); 
            
            if (string.IsNullOrEmpty(search))
                return View(new List<User>());

            try {
                string query = "SELECT * FROM Users WHERE Username LIKE '%" + search + "%'";
                var users = _db.Users.FromSqlRaw(query).ToList();
                _logger.LogInformation("Fin Search.Index, registros encontrados: {count}", users.Count); 
                return View(users);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error en Search.Index para usuario: {User}", User.Identity?.Name);
                throw;
            }
        }
    }
}