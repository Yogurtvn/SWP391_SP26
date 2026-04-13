using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Repositories;

namespace ServiceLayer.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
