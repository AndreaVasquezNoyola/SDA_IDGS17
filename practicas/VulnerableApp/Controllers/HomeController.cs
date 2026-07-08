using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VulnerableApp.Models;

namespace VulnerableApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Acceso a Home.Index");
        return View();
    }

    public IActionResult Privacy()
    {
        _logger.LogInformation("Acceso a Home.Privacy");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("Acceso a página de Error. TraceId: {TraceId}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}