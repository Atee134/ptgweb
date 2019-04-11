using Microsoft.Extensions.DependencyInjection;
using Ptg.HeightmapGenerator.HeightmapGenerators;
using Ptg.HeightmapGenerator.Interfaces;

namespace Ptg.HeightmapGenerator
{
    public static class HeightmapGeneratorDependencyLoader
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFaultHeightmapGenerator, FaultHeightmapGenerator>();
            services.AddScoped<IDiamondSquareGenerator, DiamondSquareGenerator>();
            services.AddScoped<IOpenSimplexGenerator, OpenSimplexGenerator>();
        }
    }
}
