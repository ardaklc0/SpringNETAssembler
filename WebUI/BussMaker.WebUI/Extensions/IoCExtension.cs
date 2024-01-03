using BussMaker.Infrastructure.Data;
using BussMaker.Infrastructure.Repository;
using BussMaker.Services;
using Microsoft.EntityFrameworkCore;

namespace BussMaker.WebUI.Extensions
{
    public static class IoCExtension
    {
       public static IServiceCollection AddInjections(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<IEntityRepository, EFEntityRepository>();
            services.AddDbContext<BussMakerDbContext>(opt => opt.UseSqlServer(connectionString));
            return services;
        }
    }
}
