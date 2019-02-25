using Ptg.Common.Dtos;

namespace Ptg.SplatmapGenerator.Interfaces
{
    public interface IHeightBasedSplatmapGenerator
    {
        SplatmapDto Generate(HeightmapDto heightmapDto, float lowPercent, float highPercent, float transitionPercent);
    }
}
