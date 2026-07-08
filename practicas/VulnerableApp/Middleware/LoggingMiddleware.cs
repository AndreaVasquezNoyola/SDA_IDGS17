using System.Diagnostics;

namespace VulnerableApp.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var cid = Guid.NewGuid().ToString(); 
            context.Response.Headers["X-Correlation-ID"] = cid;

            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción no controlada [CID: {CID}]", cid);
                context.Response.StatusCode = 500;
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("Método: {Method}, Ruta: {Path}, Status: {Status}, Duración: {Elapsed}ms, CID: {CID}", 
                    context.Request.Method, context.Request.Path, context.Response.StatusCode, sw.ElapsedMilliseconds, cid);
            }
        }
    }
}