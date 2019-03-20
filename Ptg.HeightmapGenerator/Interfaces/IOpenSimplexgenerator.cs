using Ptg.Common.Dtos;

namespace Ptg.HeightmapGenerator.Interfaces
{
    public interface IOpenSimplexGenerator
    {
        HeightmapDto Generate(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity);
    }
}
