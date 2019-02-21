using Ptg.Common.Dtos;

namespace Ptg.HeightmapGenerator.Interfaces
{
    public interface IDiamondSquareGenerator
    {
        HeightmapDto Generate(int width, int height, byte initialCornerValue, float offsetRange);
    }
}
