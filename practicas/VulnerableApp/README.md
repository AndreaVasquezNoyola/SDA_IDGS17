# SecureApp (Anteriormente VulnerableApp)

Este proyecto es una aplicación educativa de ASP.NET Core MVC desarrollada como parte de las prácticas de la materia de Seguridad en el Desarrollo de Aplicaciones.

La rama secure contiene la versión final del sistema, donde todas las vulnerabilidades del OWASP Top 10 detectadas en las fases anteriores han sido remediadas.

# Instrucciones de ejecución:

## 1. Requisitos Previos
- .NET 10 SDK
- SQL Server LocalDB
- Estar posicionado en la rama segura: `git checkout secure`

## 2. Configuración de Base de Datos
Base de datos con Bycript.

Ejecuta los comandos de Entity Framework para generar el esquema seguro:
   (en la terminal de tu preferencia)
   dotnet ef migrations add SecureDb
   dotnet ef database update

Para correr la aplicación
    dotnet run

Para visualizar la aplicación, ve a la url que te lance, por lo general se ve así: https://localhost:XXXX (Las X son tu puerto)
Dale click encima del link, si no funciona, da Ctrl + Enter.