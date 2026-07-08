using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioApp.Domain.Interfaces;
using PortfolioApp.Infrastructure.Data;
using PortfolioApp.Infrastructure.Repositories;

namespace PortfolioApp.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                      .EnableRetryOnFailure(3)
            ));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
