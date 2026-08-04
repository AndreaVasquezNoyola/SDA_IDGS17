# ── Stage 1: build ────────────────────────────────────────────
# Incluye SDK + código fuente + dotnet-ef.
# Esta stage se reutiliza tal cual como "migrador" (ver pentest-compose.yml,
# servicio db-migrate, target: build) porque necesita el código fuente
# y el SDK para ejecutar `dotnet ef database update`.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore VulnerableApp.csproj
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# ── Stage 2: publish ──────────────────────────────────────────
FROM build AS publish
RUN dotnet publish VulnerableApp.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Stage 3: runtime (imagen final de la app) ─────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "VulnerableApp.dll"]
