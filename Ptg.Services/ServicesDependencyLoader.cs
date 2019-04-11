using Microsoft.Extensions.DependencyInjection;
using Ptg.Services.Interfaces;
using Ptg.Services.Services;

namespace Ptg.Services
{
    public static class ServicesDependencyLoader
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGameManagerService, GameManagerService>();
            services.AddScoped<ITerrainService, TerrainService>();

            DataAccess.DataAccessDependencyLoader.AddServices(services);
            SplatmapGenerator.SplatmapGeneratorDependencyLoader.AddServices(services);
            HeightmapGenerator.HeightmapGeneratorDependencyLoader.AddServices(services);
        }
    }
}
