using Ptg.Common.Dtos;

namespace Ptg.HeightmapGenerator.Interfaces
{
    public interface IDiamondSquareGenerator
    {
        HeightmapDto Generate(int size, float offsetRange, float offsetReductionRate);
    }
}