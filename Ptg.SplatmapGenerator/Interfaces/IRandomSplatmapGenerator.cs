using Ptg.Common.Dtos;

namespace Ptg.SplatmapGenerator.Interfaces
{
    public interface IRandomSplatmapGenerator
    {
        SplatmapDto Generate(HeightmapDto heightmapDto);
    }
}
