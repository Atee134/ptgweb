using Ptg.Common.Dtos;

namespace Ptg.SplatmapGenerator.Interfaces
{
    public interface IHeightBasedSplatmapGenerator
    {
        SplatmapDto Generate(float[,] heightmap, float lowPercent, float midPercent, float highPercent);


        HeightmapDto GetTestSteepnessMap(HeightmapDto heightmapDto); // TODO remove test
    }
}
