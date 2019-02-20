using Ptg.Common.Dtos;

namespace Ptg.HeightmapGenerator.Interfaces
{
    public interface IFaultHeightmapGenerator
    {
        HeightmapDto GenerateHeightmap(int width, int height, int iterationCount, int offsetPerIteration);
    }
}
