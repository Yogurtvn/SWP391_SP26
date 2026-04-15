using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Data;
using RepositoryLayer.Interfaces;
using RepositoryUnitOfWork = RepositoryLayer.UnitOfWork.UnitOfWork;

namespace RepositoryLayer.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositoryLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<OnlineEyewearDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(OnlineEyewearDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, RepositoryUnitOfWork>();

        return services;
    }
}
