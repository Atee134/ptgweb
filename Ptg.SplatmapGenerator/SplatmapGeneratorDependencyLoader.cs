using Microsoft.Extensions.DependencyInjection;
using Ptg.SplatmapGenerator.Interfaces;
using Ptg.SplatmapGenerator.SplatmapGenerators;

namespace Ptg.SplatmapGenerator
{
    public static class SplatmapGeneratorDependencyLoader
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IHeightBasedSplatmapGenerator, HeightBasedSplatmapGenerator>();
        }
    }
}
