using Microsoft.EntityFrameworkCore;
using VulnerableApp.Data;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(Matching.FromSource("VulnerableApp.Controllers.AuthController"))
        .WriteTo.File("Logs/security-.txt", rollingInterval: RollingInterval.Day))
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// 3. CONFIGURA EL PIPELINE HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseMiddleware<VulnerableApp.Middlewares.GlobalLoggingMiddleware>();
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; frame-ancestors 'self'; form-action 'self'");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    await next();
});
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

try
{
    Log.Information("Iniciando VulnerableApp");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fallo crítico al iniciar la aplicación");
}
finally
{
    Log.CloseAndFlush();
}