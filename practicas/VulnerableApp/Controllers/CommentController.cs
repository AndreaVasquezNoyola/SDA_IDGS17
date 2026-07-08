using Microsoft.AspNetCore.Mvc;

namespace VulnerableApp.Controllers
{
    public class CommentController : Controller
    {
        private static List<string> _comments = new();
        private readonly ILogger<CommentController> _logger;

        public CommentController(ILogger<CommentController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Acceso a Comment.Index");
            return View(_comments);
        }

        [HttpPost]
        public IActionResult AddComment(string comment)
        {
            _logger.LogInformation("Intentando agregar comentario");
            if (!string.IsNullOrEmpty(comment))
            {
                _comments.Add(comment);
                _logger.LogInformation("Comentario agregado exitosamente");
            }
            return RedirectToAction("Index");
        }
    }
}