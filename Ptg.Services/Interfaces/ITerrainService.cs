using Ptg.Common.Dtos;
using Ptg.Common.Dtos.Request;
using System;

namespace Ptg.Services.Interfaces
{
    public interface ITerrainService
    {
        Guid Generate(DiamondSquareHeightmapRequestDto requestDto);
        Guid Generate(FaultHeightmapRequestDto requestDto);
        Guid Generate(RandomHeightmapRequestDto requestDto);
        Guid Generate(OpenSimplexRequestDto requestDto);
    }
}
