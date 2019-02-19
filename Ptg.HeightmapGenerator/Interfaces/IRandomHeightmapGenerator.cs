using Ptg.Common.Dtos;

namespace Ptg.HeightmapGenerator.Interfaces
{
    public interface IRandomHeightmapGenerator
    {
        HeightmapDto GenerateHeightmap(int width, int height);
    }
}
