using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableApp.Data;

namespace VulnerableApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext db, ILogger<AuthController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            _logger.LogInformation("Intento de login para usuario: {User}, IP: {IP}", username, HttpContext.Connection.RemoteIpAddress);

            try
            {
                if (username == "admin" && password == "admin")
                {
                    HttpContext.Session.SetString("User", username);
                    HttpContext.Session.SetInt32("UserId", 1);
                    return RedirectToAction("Dashboard");
                }

                string query = "SELECT * FROM Users WHERE Username = '" + username + "' AND Password ='" + password + "'";
                var user = _db.Users.FromSqlRaw(query).FirstOrDefault();
                
                if (user != null)
                {
                    HttpContext.Session.SetString("User", user.Username);
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToAction("Dashboard");
                }

                _logger.LogWarning("Login fallido para usuario: {User}", username);
                ViewBag.Error = "Usuario/contraseña inválido";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico en Login");
                throw;
            }
        }

        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login");
            var user = _db.Users.Find(userId.Value);
            return View(user);
        }

        public IActionResult Logout()
        {
            _logger.LogInformation("Cierre de sesión para usuario: {User}", HttpContext.Session.GetString("User"));
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}