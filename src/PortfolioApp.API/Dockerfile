# ─── Stage 1: Build ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY PortfolioApp.sln .
COPY src/PortfolioApp.API/PortfolioApp.API.csproj src/PortfolioApp.API/
COPY src/PortfolioApp.Application/PortfolioApp.Application.csproj src/PortfolioApp.Application/
COPY src/PortfolioApp.Domain/PortfolioApp.Domain.csproj src/PortfolioApp.Domain/
COPY src/PortfolioApp.Infrastructure/PortfolioApp.Infrastructure.csproj src/PortfolioApp.Infrastructure/
RUN dotnet restore
COPY . .
RUN dotnet publish src/PortfolioApp.API/PortfolioApp.API.csproj -c Release -o /app/publish

# ─── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "PortfolioApp.API.dll"]
