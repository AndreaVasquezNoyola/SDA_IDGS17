using Microsoft.EntityFrameworkCore;
using VulnerableApp.Data;
using Serilog;
using VulnerableApp.Middleware; // Asegúrate de importar tu middleware

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar el Logger de Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.FromLogContext()
    .CreateLogger();

// 2. Configurar el Host (Solo una vez, ANTES de builder.Build())
builder.Host.UseSerilog(); 

// 3. Servicios
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();

var app = builder.Build();

// 4. Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Importante para tus archivos estáticos
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// 5. Middleware de Logging Global (Después del Routing/Session)
app.UseMiddleware<LoggingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();