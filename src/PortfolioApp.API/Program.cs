using Microsoft.AspNetCore.Http.Features;
using PortfolioApp.API.Extensions;
using PortfolioApp.API.Middleware;
using PortfolioApp.Infrastructure;
using PortfolioApp.Infrastructure.Data;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/portfolio-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    // Services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddSwaggerDocumentation();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular", policy =>
            policy.SetIsOriginAllowed(origin =>
                origin.StartsWith("http://localhost") ||
                origin == (builder.Configuration["AllowedOrigins:Production"] ?? "https://yourportfolio.azurewebsites.net")
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
    });

    builder.Services.AddMemoryCache();

    builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
    });

    builder.Services.AddResponseCompression();

    var app = builder.Build();

    // Seed database
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await DbSeeder.SeedAsync(context);
    }

    // Middleware pipeline
    app.UseMiddleware<GlobalExceptionMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API v1");
        c.RoutePrefix = "swagger";
    });

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseResponseCompression();
    app.UseCors("AllowAngular");
    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed.");
}
finally
{
    Log.CloseAndFlush();
}
